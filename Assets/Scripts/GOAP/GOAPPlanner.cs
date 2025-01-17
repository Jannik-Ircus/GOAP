using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
    [Range(0, 4)]
    public int currentDebugLogLevel = 4;


    [SerializeField, Range(0.01f, 3)]
    private float plannerUpdateTime = 1;

    [SerializeField]
    private List<GOAPAgent> agents;

    //public List<GOAPAction> plan;
    //public List<GameObject> goals;

      /*
      * OnPlannerUpdate:
      * List of all agents for this planner
      * foreach agent:
      *     Check if agent has open goals, if no goals are to be done, do nothing
      *     if agent is running an action:
      *         get current plan of agent
      *         make new best plan for agent and compare to current agent plan
      *         if new plan is better and plan is not the same:
      *             abort current action
      *             abort current plan and get new plan
      *             start new plan
      *         else if new plan is not better or no new plan can generate:
      *             continue with old plan
      *     else if agent is not running an action:
      *         make new best plan for agent
      *         if no plan can generate:
      *             error log
      *         else:
      *             start new plan
      *    
      * StartPlanner:
      *     Start looping coroutine with OnPlannerUpdate every x seconds
      *     
      * PausePlanner
      *     Abort the looping coroutine
      *     
      * PlanOnce:
      *     Only do OnPlannerUpdate once
      *    
      * MakePlan:
      * 
      * 
      * StartPlan:
      *     Set Agent plan queue to the new plan
      *     Start first action of queue
      * ComparePlans:
      *     sum of cost for all actions for both and then compare values
      *     
      * Check if goal is achieved:
      *     Compare Agent goal states with Agent states and beliefs
      * 
      * Sort goals for priority
      * 
      * Debuglogging
      *     Method for different debug categories
      *     Depending on category the debug will be displayed or not
      *     easier to enable/disable debug logging
      */


    private void OnPlannerUpdate()
    {
        DebugMessage("Starting OnPlannerUpdate", 3);
        foreach(GOAPAgent agent in agents)
        {
            if (agent == null) continue;
            if (!agent.isActiveAndEnabled) continue;
            DebugMessage("Agent: " + agent.name + " is getting checked", 3);
            //if(!agent.agentStates.GetStates().ContainsKey("testState")) agent.agentStates.AddState("testState", 1);
            //Check if agent has open goals, if no goals are to be done, do nothing
            List<GoalState> agentGoals = new List<GoalState>(agent.goalStates);
            
            foreach(KeyValuePair<string, int> agentState in agent.agentStates.GetStates())
            {
                
                int currentGoal = GoalStatesIncludeKey(agentGoals, agentState.Key); //TODO: make a method and dont reuse same code twice
                if (currentGoal >= 0)
                {
                    bool GoalFulfilled = false;
                    switch (agentGoals[currentGoal].goalType)
                    {
                        default:
                        case GoalType.equals:
                            if (agentState.Value == agentGoals[currentGoal].value)
                                GoalFulfilled = true;
                                break;
                        case GoalType.less:
                            if (agentState.Value <= agentGoals[currentGoal].value)
                                GoalFulfilled = true;
                            break;
                        case GoalType.greater:
                            if (agentState.Value >= agentGoals[currentGoal].value)
                                GoalFulfilled = true;
                            break;
                    }

                    if(GoalFulfilled)
                    {
                        agentGoals.RemoveAt(currentGoal);
                    }
                    
                }
            }

            foreach(KeyValuePair<string, int> agentPerceivedWorldState in agent.agentStates.GetStates()) //TODO: make a method and dont reuse same code twice
            {
                int currentGoal = GoalStatesIncludeKey(agentGoals, agentPerceivedWorldState.Key);
                if (currentGoal >= 0)
                {
                    bool GoalFulfilled = false;
                    switch (agentGoals[currentGoal].goalType)
                    {
                        default:
                        case GoalType.equals:
                            if (agentPerceivedWorldState.Value == agentGoals[currentGoal].value)
                                GoalFulfilled = true;
                            break;
                        case GoalType.less:
                            if (agentPerceivedWorldState.Value <= agentGoals[currentGoal].value)
                                GoalFulfilled = true;
                            break;
                        case GoalType.greater:
                            if (agentPerceivedWorldState.Value >= agentGoals[currentGoal].value)
                                GoalFulfilled = true;
                            break;
                    }

                    if (GoalFulfilled)
                    {
                        agentGoals.RemoveAt(currentGoal);
                    }

                }
            }
            
            if(agentGoals.Count > 0)
            {
                DebugMessage("Agent: " + agent.name + " has open goals!", 3);
                Queue<GOAPAction> currentAgentPlan = null;
                if (agent.GetCurrentPlan()!=null)currentAgentPlan = new Queue<GOAPAction>(agent.GetCurrentPlan());

                Queue<GOAPAction> generatedPlan = GenerateNewPlan(agent, agentGoals);
                if (generatedPlan == null)
                {
                    DebugMessage("No plan could be generated for any goal on agent: " + agent, 1);
                    continue;
                }
                Queue<GOAPAction> newPlan = new Queue<GOAPAction>(generatedPlan);
                
                foreach(GOAPAction ac in newPlan)
                {
                    DebugMessage(ac + " is an action", 3);
                }

                if(currentAgentPlan != null)
                {
                    if (!agent.currentlyRunning)
                    {
                        DebugMessage("Agent: " + agent.name + " is not running. Restarting", 1);
                        agent.SetCurrentPlan(newPlan);
                        if (agent.IsAgentCurrentlyRunning()) agent.AbortPlan();
                        agent.StartPlan();
                    }
                    if(GetPlanGoalPriority(currentAgentPlan, agent) >= GetPlanGoalPriority(newPlan, agent))
                    {
                        if (PlanEqualsPlan(currentAgentPlan, newPlan))
                        {
                            DebugMessage("new plan is current plan", 2);
                            continue;
                        }
                        if (GetPlanCost(currentAgentPlan, agent) <= GetPlanCost(newPlan, agent))
                        {
                            DebugMessage("new plan worse than current plan", 2);
                            continue;
                        } 
                    }
                    
                    
                    
                }
                if (agent.IsAgentCurrentlyRunning()) agent.AbortPlan();
                agent.SetCurrentPlan(newPlan);
                
                agent.StartPlan();

            } else
            {
                DebugMessage("Agent: " + agent.name + " has all goals fulfilled!", 3);
            }

            
        }
    }

    private Queue<GOAPAction> GenerateNewPlan(GOAPAgent agent, List<GoalState> agentGoals)
    {
        //sort goals according to priority
        //try to find plan for each goal. Once a plan is found, do that plan and stop looking for other plans
        //
        //Build graph network for the available actions like https://excaliburjs.com/blog/goal-oriented-action-planning/
        //search for cheapest plan inside  that graph network, for example with Dijkstra algo
        //compare that to the currentPlan, if they have the same cost, use the currentPlan

        //get list of all currently usable actions as starting nodes for the Graph
        List<GOAPAction> usableActions = new List<GOAPAction>();
        foreach (GOAPAction action in agent.actions)
        {
            if (action.IsAchievable(agent)) usableActions.Add(action);
        }

        List<GOAPNode> graph = new List<GOAPNode>();
        GOAPNode start = new GOAPNode(null, 0, GOAPWorld.Instance.GetWorld().GetStates(), agent.agentStates.GetStates(), null);
        Dictionary<string, int> goal = new Dictionary<string, int>();
        //sort goals and make try to make plan for every goal until one is done
        List<GoalState> sortedGoals = new List<GoalState>(agentGoals);
        sortedGoals.Sort((x, y) => y.priority.CompareTo(x.priority));
        foreach(GoalState goalState in sortedGoals)
        {
            if(GoalAchieved(goalState, agent.agentStates.states))
            {
                DebugMessage("Goal: " + goalState.key + " is already fulfilled!", 0);
                continue;
            }

            //start building the graph
            bool success = BuildGraphFromBottom(start, graph, usableActions, goalState);
            if (!success)
            {
                DebugMessage("No plan found for agent: " + agent.name, 2);
                continue;
            }
            //DebugMessage("Found the following graph: ", 0);
            //DebugGraph(graph, -1, true);
            /*Debug.Log("-------------------------------------------------");
            Debug.Log("GraphCount: " + graph.Count);
            foreach(GOAPNode graphNode in graph)
            {
                Debug.Log("Node cost: " + graphNode.cost);
            }*/
            if (graph.Count > 0)
            {
                GOAPNode cheapestNode = graph[0];
                foreach (GOAPNode node in graph)
                {
                    if (node.cost < cheapestNode.cost) cheapestNode = node;
                }
                Queue<GOAPAction> planToReturn = new Queue<GOAPAction>(GeneratePlanFromNodeFromBottom(cheapestNode));

                return planToReturn;
            }
            else
            {
                DebugMessage("Error. No plan could be generated", 1);
                continue;
            }
        }


        return null;
        
    }

    private bool BuildGraph(GOAPNode parent, List<GOAPNode> graph, List<GOAPAction> usableActions, GoalState goal)
    {
        bool foundPath = false;

        //generate path for every action. like leaves on a tree. Until goal is found for every branch or no actions are available anymore
        foreach(GOAPAction action in usableActions)
        {
            if(graph.Count>=1) if (graph[graph.Count - 1].cost <= parent.cost+action.cost) continue; //TODO: fix more performance stuff. This will automatically ignore plans, which are expected to be higher than others
            if (action.IsAchievableGiven(parent.state))
            {
                //get states for when the action is done, to check the state after the action is completed
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(GOAPWorldState eff in action.afterEffects)
                {
                    if (!currentState.ContainsKey(eff.key)) currentState.Add(eff.key, eff.value);
                    else if (currentState.ContainsKey(eff.key) && eff.value != currentState[eff.key]) //if state is already part of agentState, but the value is not the same as the goal, then use the new value
                    {
                        currentState[eff.key] = eff.value;
                    }
                }

                GOAPNode node = new GOAPNode(parent, parent.cost + action.cost, currentState, action); //generate node for this action
                //check if this node can fulfill the goal
                //Debug.Log("here");
                if(GoalAchieved(goal, currentState))
                {
                    //Debug.Log("goalAchieved ");
                    //Add node to graph to get a complete List of actions which can fulfill the goal
                    graph.Add(node);
                    foundPath = true;
                } else
                {
                    //check if the goal can be fulfilled by checking actions which can be done, after this one
                    //basically, go down the tree and do the same thing as before for every action. -> inverse
                    List<GOAPAction> subset = ActionsSubset(usableActions, action);
                    if (subset.Count <= 0) DebugMessage("Graph ends at node: " + node, 2);
                    bool found = BuildGraph(node, graph, subset, goal);
                    if(found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    private bool BuildGraphFromBottom(GOAPNode parent, List<GOAPNode> graph, List<GOAPAction> usableActions, GoalState goal)
    {
        bool foundPath = false;

        //generate path for every action. like leaves on a tree. Until goal is found for every branch or no actions are available anymore
        foreach (GOAPAction action in usableActions)
        {
            Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
            foreach (GOAPWorldState eff in action.afterEffects)
            {
                if (!currentState.ContainsKey(eff.key)) currentState.Add(eff.key, eff.value);
                else if (currentState.ContainsKey(eff.key) && eff.value != currentState[eff.key]) //if state is already part of agentState, but the value is not the same as the goal, then use the new value
                {
                    currentState[eff.key] = eff.value;
                }
            }
            GOAPNode node = new GOAPNode(parent, parent.cost + action.cost, currentState, action);

            if(GoalAchieved(goal, currentState))
            {
                if (action.IsAchievableGiven(currentState))
                {
                    graph.Add(node);
                    foundPath = true;
                } else
                {
                    List<GOAPAction> subset = ActionsSubset(usableActions, action);
                    if (subset.Count <= 0) DebugMessage("Graph ends at node: " + node, 2);
                    bool found = BuildGraphFromBottom(node, graph, subset, action.preConditions);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }

        }

        return foundPath;
    }

    

    private bool BuildGraphFromBottom(GOAPNode parent, List<GOAPNode> graph, List<GOAPAction> usableActions, GOAPWorldState[] preConditions)
    {
        bool foundPath = false;

        foreach (GOAPAction action in usableActions)
        {
            Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
            foreach (GOAPWorldState eff in action.afterEffects)
            {
                if (!currentState.ContainsKey(eff.key)) currentState.Add(eff.key, eff.value);
                else if (currentState.ContainsKey(eff.key) && eff.value != currentState[eff.key]) //if state is already part of agentState, but the value is not the same as the goal, then use the new value
                {
                    currentState[eff.key] = eff.value;
                }
            }
            GOAPNode node = new GOAPNode(parent, parent.cost + action.cost, currentState, action);
            GOAPWorldState[] newConditions = CheckPreConditions(preConditions, currentState);

            if(newConditions.Length <= 0) //plan found with no more conditions
            {
                if(action.IsAchievableGiven(currentState))
                {
                    graph.Add(node);
                    foundPath = true;
                } else
                {
                    List<GOAPAction> subset = ActionsSubset(usableActions, action);
                    if (subset.Count <= 0) DebugMessage("Graph ends at node: " + node, 2);
                    bool found = BuildGraphFromBottom(node, graph, subset, action.preConditions);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
                
            } else if(newConditions.Length < preConditions.Length) //plan found with less preConditions. -> action is needed for other action
            {
                List<GOAPAction> subset = ActionsSubset(usableActions, action);
                if (subset.Count <= 0) DebugMessage("Graph ends at node: " + node, 2);

                bool found = BuildGraphFromBottom(node, graph, subset, CombineConditions(newConditions, action.preConditions));
                if (found)
                {
                    foundPath = true;
                }
            }

        }

        return foundPath;
    }

    private GOAPWorldState[] CombineConditions(GOAPWorldState[] oldConditions, GOAPWorldState[] newConditions)
    {
        List<GOAPWorldState> combinedConditions = new List<GOAPWorldState>(oldConditions);

        foreach(GOAPWorldState condition in newConditions)
        {
            if (!combinedConditions.Contains(condition)) combinedConditions.Add(condition);
        }

        return combinedConditions.ToArray();
    }

    private GOAPWorldState[] CheckPreConditions(GOAPWorldState[] preConditions, Dictionary<string, int> currentState)
    {
        List<GOAPWorldState> conditions = new List<GOAPWorldState>(preConditions);
        List<GOAPWorldState> conditionsToRemove = new List<GOAPWorldState>();
        foreach (GOAPWorldState condition in conditions)
        {
            if (!currentState.ContainsKey(condition.key)) continue;
            if (currentState[condition.key] == condition.value) //the currentState has the correct key and the same value as needed in the preConditions
            {
                conditionsToRemove.Add(condition);
            }
        }

        foreach(GOAPWorldState conditionToRemove in conditionsToRemove)
        {
            if (conditions.Contains(conditionToRemove)) conditions.Remove(conditionToRemove);
        }

        return conditions.ToArray();
    }

    private bool GoalAchieved(GoalState goal, Dictionary<string, int> state)
    {
        bool goalAchieved = false;
        if(state.ContainsKey(goal.key))
        {
            switch(goal.goalType)
            {
                default:
                case GoalType.equals:
                    if (state[goal.key] == goal.value) goalAchieved = true;
                    break;
                case GoalType.less:
                    if (state[goal.key] <= goal.value) goalAchieved = true;
                    break;
                case GoalType.greater:
                    if (state[goal.key] >= goal.value) goalAchieved = true;
                    break;
            }
        }

        return goalAchieved;
    }

    private List<GOAPAction> ActionsSubset(List<GOAPAction> actions, GOAPAction actionToRemove)
    {
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach(GOAPAction action in actions)
        {
            if (!action.Equals(actionToRemove)) subset.Add(action);
        }
        return subset;
    }

    private Queue<GOAPAction> GeneratePlanFromNode(GOAPNode node)
    {
        if (node.action == null) return null;
        List<GOAPAction> actions = new List<GOAPAction>();
        AddNextActionsToList(actions, node);
        Queue<GOAPAction> queueToReturn = new Queue<GOAPAction>();
        for(int i = actions.Count-1; i>=0; i--)
        {
            queueToReturn.Enqueue(actions[i]);
        }
        return queueToReturn;
    }

    private Queue<GOAPAction> GeneratePlanFromNodeFromBottom(GOAPNode node)
    {
        if (node.action == null) return null;
        List<GOAPAction> actions = new List<GOAPAction>();
        AddNextActionsToList(actions, node);
        Queue<GOAPAction> queueToReturn = new Queue<GOAPAction>();
        for (int i = 0; i <= actions.Count - 1; i++)
        {
            queueToReturn.Enqueue(actions[i]);
        }
        return queueToReturn;
    }

    private void AddNextActionsToList(List<GOAPAction> actions, GOAPNode node)
    {
        if(node.action!=null)actions.Add(node.action);
        if(node.parent != null)
        {
            AddNextActionsToList(actions, node.parent);
        }
    }

    public void StartPlanner()
    {
        if (IsInvoking("OnPlannerUpdate")) return;
        InvokeRepeating("OnPlannerUpdate", 0, plannerUpdateTime);
    }

    public void StopPlanner()
    {
        agents[0].AbortPlan();
        if (IsInvoking("OnPlannerUpdate"))
        {
            CancelInvoke("OnPlannerUpdate");
        } else
        {
            DebugMessage("currently not invoking!" + plannerUpdateTime, 3);
        }
    }

    public void DoPlannerOnce()
    {
        OnPlannerUpdate();
    }

    private bool PlanEqualsPlan(Queue<GOAPAction> plan1, Queue<GOAPAction> plan2)
    {
        if (plan1.Count != plan2.Count) return false;
        GOAPAction[] array1 = plan1.ToArray();
        GOAPAction[] array2 = plan2.ToArray();
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i].action != array2[i].action) return false;
        }
        return true;
    }

    private float GetPlanCost(Queue<GOAPAction> plan, GOAPAgent agent)
    {
        float cost = 0;
        foreach(GOAPAction action in plan)
        {
            cost += action.GetCost(agent);
        }
        return cost;
    }

    private float GetPlanGoalPriority(Queue<GOAPAction> plan, GOAPAgent agent)
    {
        float priority = 0;
        GOAPAction[] planArray = plan.ToArray();
        foreach(GoalState goalState in agent.goalStates)
        {
            foreach (GOAPWorldState worldState in planArray[planArray.Length-1].afterEffects)
                if (goalState.key == worldState.key) priority = goalState.priority;
        }

        return priority;
    }

    private void DebugMessage(string message, int debugID) //0-show always, 1-Error, 2-Warnings, 3-messages
    {
        if (currentDebugLogLevel < debugID) return;
        switch (debugID)
        {
            default:
            case 0:
            case 3:
                Debug.Log(message);
                break;
            case 1:
                Debug.LogError(message);
                break;
            case 2:
                Debug.LogWarning(message);
                break;
        }
    }

    private void DebugGraph(List<GOAPNode> graph, int index, bool firstTime)
    {
        if(firstTime)DebugMessage("Generated " + graph.Count + " paths.", 0);
        if(index == -1)
        {
            int i = 0;
            foreach(GOAPNode node in graph)
            {
                if (firstTime) DebugMessage("Path " + i + ": ", 0);
                DebugMessage("Node action: " + node.action, 0);
                if (node.parent.action != null)
                {
                    List<GOAPNode> parentGraph = new List<GOAPNode>();
                    parentGraph.Add(node.parent);
                    DebugGraph(parentGraph, -1, false);
                    
                }
                if (firstTime) DebugMessage("---------------------------------", 0);
                if (firstTime) i++;
            }
            return;
        }
    }

    private int GoalStatesIncludeKey(List<GoalState> goalState, string key)
    {
        int index = 0;
        foreach(GoalState gs in goalState)
        {
            if (gs.key == key)return index;
            index++;
        }
        return -1;
    }

    public void SetAgents(List<GOAPAgent> newAgents)
    {
        if (IsInvoking("OnPlannerUpdate"))
        {
            Debug.Log("Planner is already running. Cant change agents at runtime!");
            return;
        }

        agents = new List<GOAPAgent>(newAgents);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoalType
{
    equals = 0,
    greater = 1,
    less = 2,
}

[System.Serializable]
public class GoalState
{
    public string key;
    public GoalType goalType;
    public int value;
    public float priority;
}

public class GOAPAgent : MonoBehaviour
{
    [Header("Agent State Updater")]
    [Tooltip("GOAPAgentStateUpdater script which will update the agent states. Completely optional.")]
    public GOAPAgentStateUpdater agentStateUpdater;
    public float agentStateUpdateRate = 1f;
    //[Tooltip("true - updater will always run. false - updater only runs when agent is running.")]
    //public bool alwaysRunAgentStateUpdater = true;

    public GOAPWorldStates agentStates = new GOAPWorldStates();
    [Header("Agent Actions")]
    public List<GOAPAction> actions;

    private Queue<GOAPAction> currentPlan;
    private GOAPAction currentAction;
    //public bool isRunning = false;

    [Header("Agent Goals")]
    [SerializeField]
    public List<GoalState> goalStates;

    public bool ShowDebugLogs = false;
    public bool currentlyRunning = false;

    private void Awake()
    {
        if(agentStateUpdater != null)
        {
            InvokeRepeating("UpdateAgentStates", agentStateUpdateRate, agentStateUpdateRate);
        }
    }

    public bool SetCurrentPlan(Queue<GOAPAction> newPlan)
    {
        if (currentAction != null && currentAction.IsRunning()) currentAction.AbortAction(this);
        currentPlan = null;
        currentAction = null;
        currentPlan = new Queue<GOAPAction>(newPlan);
        return true;
    }

    public Queue<GOAPAction> GetCurrentPlan()
    {
        
        if (currentAction != null)
        {
            Queue<GOAPAction> QueueToReturn = new Queue<GOAPAction>();
            QueueToReturn.Enqueue(currentAction);
            foreach(GOAPAction action in currentPlan)
            {
                QueueToReturn.Enqueue(action);
            }
            return QueueToReturn;
        }
        //Debug.LogError("No plan");
        return null;
    }

    public void StartPlan()
    {
        if(ShowDebugLogs)Debug.Log("StartPlan " + currentPlan + "  " + currentPlan.Count);
        if (currentAction != null && currentAction.IsRunning())
        {
            if (ShowDebugLogs) Debug.Log("GOAP: Agent " + name + " is already running plan.");
            return;
        }
        else if (currentAction != null && !currentAction.IsRunning())
        {
            if(!currentAction.IsAchievable())
            {
                Debug.LogError("GOAP: Agent " + name + " Action: " + currentAction.actionName + " is not achievable!");
            }
            StartCoroutine(StartAction());
        } else if(currentAction == null)
        {
            if (currentPlan.Count <= 0)
            {
                Debug.LogError("GOAP: Agent " + name + " has empty plan! Cant start empty plan!");
                return;
            }
            currentAction = currentPlan.Dequeue();
            StartCoroutine(StartAction());
        }
    }

    private IEnumerator StartAction()
    {
        if (ShowDebugLogs) Debug.Log("StartAction");
        if(currentAction == null)
        {
            Debug.LogError("GOAP: Agent " + name + " No current action!");
            yield return new WaitForSeconds(0);
        } else
        {
            GOAPActionClass currentActionClass = currentAction.GetGOAPActionClassFromCustom();
            if (!currentActionClass.isRunning && currentAction.IsAchievable())
            {

                if (ShowDebugLogs) Debug.Log("GOAP: Agent " + name + " is starting action: " + currentAction.actionName);
                currentActionClass.isRunning = true;
                currentActionClass.PrePerform(this);
                currentlyRunning = true;
                yield return currentActionClass.PerformAction(this, currentAction.goal, currentAction.goalTag);
                currentActionClass.PostPerform(this);
                currentActionClass.isRunning = false;
                currentlyRunning = false;
                if (ShowDebugLogs) Debug.Log("GOAP: Agent " + name + " finished action: " + currentAction.actionName);
                StartNextAction();
            } else
            {
                Debug.LogError("GOAP: Agent " + name + " action: " + currentAction.name + " is not achievable or already running.");
            }
        }
    }

    private void StartNextAction()
    {
        if(currentPlan.Count <= 0)
        {
            if (ShowDebugLogs) Debug.Log("GOAP: Agent " + name + " finished plan. " + currentPlan + "  " + currentPlan.Count);
            currentAction = null;
            return;
        }
        currentAction = currentPlan.Dequeue();
        if(!currentAction.IsAchievable())
        {
            Debug.LogWarning("GOAP: Agent " + name + " action: " + currentAction.name + " is not achievable. Aborting.");
            return;
        }
        StartCoroutine(StartAction());
    }

    public void StopAction()
    {
        if(currentAction == null)
        {
            Debug.LogError("GOAP: Agent " + name + " has no current Action to stop");
            return;
        }
        GOAPActionClass currentActionClass = currentAction.GetGOAPActionClassFromCustom();
        StopAllCoroutines();
        currentAction.AbortAction(this);
        StopCoroutine(StartAction());
        StopCoroutine(currentActionClass.PerformAction(this, currentAction.goal, currentAction.goalTag));
        currentActionClass.isRunning = false;
        currentlyRunning = false;
    }

    public void AbortPlan()
    {
        StopAction();
        currentAction = null;
        //currentPlan = null;
    }

    private void UpdateAgentStates()
    {
        agentStateUpdater.UpdateAgentStates(this);
    }

    public GOAPAction GetCurrentAction()
    {
        return currentAction;
    }

    public bool IsAgentCurrentlyRunning()
    {
        if (currentAction == null) return false;
        GOAPActionClass currentActionClass = currentAction.GetGOAPActionClassFromCustom();
        return currentActionClass.isRunning;
    }

    public void SetGoalPriority(string goalName, int newPriority)
    {
        foreach(GoalState goal in goalStates)
        {
            if (goal.key == goalName) goal.priority = newPriority;
        }
    }

    public GoalState GetGoalState(string goalName)
    {
        foreach(GoalState goal in goalStates)
        {
            if (goal.key == goalName) return goal;
        }
        return null;
    }
}

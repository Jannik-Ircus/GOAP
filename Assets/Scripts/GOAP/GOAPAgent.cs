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
    //public Dictionary<string, int> agentStates = new Dictionary<string, int>();
    public GOAPWorldStates agentStates = new GOAPWorldStates();
    public List<GOAPAction> actions;
    //public Dictionary<string, int> goals;

    private Queue<GOAPAction> currentPlan;
    private GOAPAction currentAction;
    public bool isRunning = false;

    [SerializeField]
    public List<GoalState> goalStates;

    private void Awake()
    {
        /*foreach(GoalState goalState in goalStates)
        {
            if(!goals.ContainsKey(goalState.key)) goals.Add(goalState.key, goalState.value);
        }*/
    }

    public bool SetCurrentPlan(Queue<GOAPAction> newPlan)
    {
        foreach(GOAPAction action in newPlan)
        {
            /*if(!actions.Contains(action))
            {
                Debug.LogError("GOAP: Agent " + name + " does not have necessary action: " + action);
                Debug.LogError("GOAP: Agent " + name + " is aborting SetCurrentPlan");
                return false;
            }*/
        }
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
        Debug.Log("StartPlan " + currentPlan + "  " + currentPlan.Count);
        if (currentAction != null && currentAction.IsRunning())
        {
            Debug.Log("GOAP: Agent " + name + " is already running plan.");
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
        Debug.Log("StartAction");
        if(currentAction == null)
        {
            Debug.LogError("GOAP: Agent " + name + " No current action!");
            yield return new WaitForSeconds(0);
        } else
        {
            GOAPActionClass currentActionClass = currentAction.GetGOAPActionClassFromCustom();
            if (!currentActionClass.isRunning && currentAction.IsAchievable())
            {
                
                Debug.Log("GOAP: Agent " + name + " is starting action: " + currentAction.actionName);
                currentActionClass.isRunning = true;
                yield return currentActionClass.PerformAction(this, currentAction.goal, currentAction.goalTag);
                currentActionClass.isRunning = false;
                Debug.Log("GOAP: Agent " + name + " finished action: " + currentAction.actionName);
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
            Debug.Log("GOAP: Agent " + name + " finished plan. " + currentPlan + "  " + currentPlan.Count);
            currentAction = null;
            return;
        }
        currentAction = currentPlan.Dequeue();
        if(!currentAction.IsAchievable())
        {
            Debug.LogError("GOAP: Agent " + name + " action: " + currentAction.name + " is not achievable. Aborting.");
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
        currentAction.AbortAction(this);
        StopCoroutine(StartAction());
        StopCoroutine(currentActionClass.PerformAction(this, currentAction.goal, currentAction.goalTag));
    }

    public void AbortPlan()
    {
        StopAction();
        currentPlan = null;
        currentAction = null;
    }

    public GOAPAction GetCurrentAction()
    {
        return currentAction;
    }
}

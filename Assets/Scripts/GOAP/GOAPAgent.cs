using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public Dictionary<string, int> agentStates;
    public GOAPWorldStates perceivedWorldStates;
    public List<GOAPAction> actions;
    public Dictionary<string, int> goals;

    private Queue<GOAPAction> currentPlan;
    private GOAPAction currentAction;
    public bool active = false;

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
        currentPlan = newPlan;
        return true;
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
                yield return currentActionClass.PerformAction(this, currentAction.goal);
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
        StopCoroutine(currentActionClass.PerformAction(this, currentAction.goal));
    }
}

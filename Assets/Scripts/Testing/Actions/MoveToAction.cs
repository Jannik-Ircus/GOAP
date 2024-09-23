using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MoveToAction : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent)
    {;
        Debug.Log("Abort Action");
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if(navAgent != null)navAgent.isStopped = true;
    }

    public override float GetCost()
    {
        return -1;
    }

    public override bool IsAchievable()
    {
        if (GOAPWorld.Instance.GetWorld().HasState("goal")) return true;
        return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        if (isRunning) yield return new WaitForSeconds(0);
        //Debug.Log("Perform action started");
        isRunning = true;
        GameObject agentObject = agent.gameObject;
        if (agentObject == null)
        {
            LogError("failed to find GameObject of agent");
            yield return new WaitForSeconds(0);
        }

        if (goal == null)
        {
            goal = GameObject.FindGameObjectWithTag(goalTag);
            if(goal == null)
            {
                LogError("failed to find GameObject of goal");
                yield return new WaitForSeconds(0);
            }
        }

        NavMeshAgent navAgent = agentObject.GetComponent<NavMeshAgent>();
        if(navAgent == null)
        {
            Debug.LogError("failed to find NavMeshAgent on agent: " + agentObject.name);
        }
        navAgent.isStopped = false;
        navAgent.SetDestination(goal.transform.position);
        //Debug.Log("Goal is " + goal.name);
        yield return new WaitForSeconds(2);
        while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning)
        {
            yield return null;
        }
        //Debug.Log("Agent reached destination");
        navAgent.isStopped = true;
        isRunning = false;
    }

   

    public override void PostPerform()
    {
        
    }

    public override void PrePerform()
    {
        
    }
}

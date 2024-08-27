using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MoveToAction", menuName = "GOAP/ActionClass/MoveToAction")]
public class MoveToAction : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent, GameObject goal)
    {
        Debug.Log("Abort Action");
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if(navAgent != null)navAgent.isStopped = true;
    }

    public override bool IsAchievable()
    {
        return true;
    }

    public override void PerformAction(GOAPAgent agent, GameObject goal)
    {
        Debug.Log("Perform action started");
        isRunning = true;
        GameObject agentObject = agent.gameObject;
        if (agentObject == null)
        {
            LogError("failed to find GameObject of agent: " + agent);
            //yield return new WaitForSeconds(0);
            return;
        }

        NavMeshAgent navAgent = agentObject.GetComponent<NavMeshAgent>();
        if(navAgent == null)
        {
            Debug.LogError("failed to find NavMeshAgent on agent: " + agentObject.name);
        }

        navAgent.SetDestination(goal.transform.position);
        //while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning)
        //{
            //yield return null;
        //}
        Debug.Log("Agent reached destination");
    }

   

    public override void PostPerform()
    {
        
    }

    public override void PrePerform()
    {
        
    }
}

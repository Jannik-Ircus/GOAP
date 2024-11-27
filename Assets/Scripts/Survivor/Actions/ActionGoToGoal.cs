using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionGoToGoal : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent)
    {
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable()
    {
        return true;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;

        GameObject target = GetClosestHome(agent.gameObject, goalTag);
        if (target != null)
        {
            NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                yield return new WaitForEndOfFrame();
            }

            navAgent.isStopped = false;
            navAgent.SetDestination(target.transform.position);

            yield return new WaitForSeconds(1);
            while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning && target != null)
            {
                yield return null;
            }



            navAgent.isStopped = true;
        }
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
        
    }

    public override void PrePerform(GOAPAgent agent)
    {
        
    }

    private GameObject GetClosestHome(GameObject agent, string goalTag)
    {
        GameObject[] homes = GameObject.FindGameObjectsWithTag(goalTag);
        GameObject closestHome = null;
        float closestAgentDistance = 10000;
        foreach (GameObject ag in homes)
        {
            float distance = Vector3.Distance(ag.transform.position, agent.transform.position);
            if (distance < closestAgentDistance)
            {
                closestAgentDistance = distance;
                closestHome = ag;
            }
        }

        return closestHome;
    }
}

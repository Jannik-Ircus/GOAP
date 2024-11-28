using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionRunAway : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent)
    {
        SetAgentSpeed(agent.gameObject, 3.5f);
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable(GOAPAgent agent)
    {
        return true;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;

        Vector3 target = GetRandomPointNearAgent(agent.gameObject);
        if (target != null)
        {
            NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                yield return new WaitForEndOfFrame();
            }

            navAgent.isStopped = false;
            navAgent.SetDestination(target);

            int currentLoop = 0;
            yield return new WaitForSeconds(1);
            while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning && target != Vector3.zero && currentLoop <= 10)
            {
                yield return null;
                currentLoop++;
            }



            navAgent.isStopped = true;
        }
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
        SetAgentSpeed(agent.gameObject, 3.5f);
    }

    public override void PrePerform(GOAPAgent agent)
    {
        SetAgentSpeed(agent.gameObject, 7f);
    }

    private void SetAgentSpeed(GameObject agent, float speed)
    {
        NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
            return;
        }

        navAgent.speed = speed;
    }

    private Vector3 GetRandomPointNearAgent(GameObject agent)
    {
        Vector3 randomPoint = Random.insideUnitCircle;

        randomPoint *= 50;
        randomPoint += agent.transform.position;

        Vector3 pointToReturn =  Vector3.zero;
        Ray ray = new Ray(randomPoint, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            pointToReturn = hit.point;
        }

        return pointToReturn;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionEatBerry : GOAPActionClass
{

    private SurvivorBerry berryToEat;
    public override void AbortAction(GOAPAgent agent)
    {
        if (berryToEat != null) berryToEat.ClaimBerry(null);
        
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;
    }

    public override float GetCost(GOAPAgent agent)
    {
        SurvivorBerry berry = GetNearestBerry(agent.gameObject);
        if (berry == null) return -1;
        int costToReturn = (int)Mathf.Abs(Vector3.Distance(agent.gameObject.transform.position, berry.transform.position) / 2);
        return costToReturn;
    }

    public override bool IsAchievable(GOAPAgent agent)
    {
        
        GameObject[] berry = GameObject.FindGameObjectsWithTag("Berry");
        if (berry.Length <= 0) return false;
        foreach(GameObject ber in berry)
        {
            SurvivorBerry sBer = ber.GetComponent<SurvivorBerry>();
            if (sBer == null) return false;
            if (!sBer.IsClaimed(agent.gameObject) && !sBer.isStored) return true;
        }
        return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if (agent == null)
        {
            Debug.LogError("No agent found on " + this);
            yield return new WaitForEndOfFrame();
        }
        SurvivorBerry berry = GetNearestBerry(agent.gameObject);
        if (berry != null)
        {
            berry.ClaimBerry(agent.gameObject);
            berryToEat = berry;
            NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                yield return new WaitForEndOfFrame();
            }

            navAgent.isStopped = false;
            navAgent.SetDestination(berry.transform.position);

            yield return new WaitForSeconds(1);
            while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);
            berry.EatBerry(agent);
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

    private SurvivorBerry GetNearestBerry(GameObject agentGameObject)
    {
        GameObject[] berries = GameObject.FindGameObjectsWithTag("Berry");
        if (berries.Length == 0) return null;

        SurvivorBerry nearestBerry = null;
        float closestDistance = 10000;
        foreach(GameObject berry in berries)
        {
            SurvivorBerry sBerry = berry.GetComponent<SurvivorBerry>();
            if (sBerry == null) continue;
            if (sBerry.IsClaimed(agentGameObject) || sBerry.isStored) continue;
            if (Vector3.Distance(agentGameObject.gameObject.transform.position, berry.transform.position) < closestDistance)
            {
                nearestBerry = sBerry;
                closestDistance = Vector3.Distance(agentGameObject.gameObject.transform.position, berry.transform.position);
            }
        }

        return nearestBerry;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ActionHuntAgent : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent)
    {
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;
    }

    public override float GetCost(GOAPAgent agent)
    {
        GameObject ag = GetClosestAgent(agent.gameObject);
        if (ag == null) return 100;
        return Mathf.Abs(Vector3.Distance(ag.transform.position, ag.gameObject.transform.position) / 2);
    }

    public override bool IsAchievable(GOAPAgent agent)
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        if (agents.Length >= 1) return true;
        return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        SurvivorEnemyUpdater enemyUpdater = agent.GetComponent<SurvivorEnemyUpdater>();
        if(enemyUpdater==null)
        {
            Debug.LogError("Cant find SurvivorEnemyUpdater on " + agent.gameObject.name);
            yield return null;
        }
        enemyUpdater.StartHunt(agent);

        GameObject target = GetClosestAgent(agent.gameObject);
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
            while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning && target!=null)
            {
                navAgent.SetDestination(target.transform.position);
                yield return null;
            }

            if(Vector3.Distance(agent.gameObject.transform.position, target.transform.position) < 2)
            {
                
                if (enemyUpdater != null)
                {
                    if(target.GetComponent<GOAPAgent>()!=null) enemyUpdater.Attack(target.GetComponent<GOAPAgent>(), agent);
                    else if(target.GetComponent<SurvivorAgentUpdaterBT>()!=null) enemyUpdater.AttackBT(target.GetComponent<SurvivorAgentUpdaterBT>(), agent);
                }
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

    private GameObject GetClosestAgent(GameObject hunter)
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        GameObject closestAgent = null;
        float closestAgentDistance = 10000;
        foreach (GameObject ag in agents)
        {
            float distance = Vector3.Distance(ag.transform.position, hunter.transform.position);
            if (distance < closestAgentDistance)
            {
                closestAgentDistance = distance;
                closestAgent = ag;
            }
        }

        return closestAgent;
    }
}

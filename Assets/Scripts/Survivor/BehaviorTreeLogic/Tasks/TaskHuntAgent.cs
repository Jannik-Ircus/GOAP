using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskHuntAgent : BTNode
{
    private SurvivorEnemyBTUpdater agent;
    private NavMeshAgent navAgent;
    private SurvivorAgentUpdaterBT target;

    public TaskHuntAgent(SurvivorEnemyBTUpdater agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskHuntAgent");
            return state;
        }

        SurvivorEnemyBTUpdater enemy = agent.GetComponent<SurvivorEnemyBTUpdater>();
        if (enemy == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        if (target == null) target = GetClosestAgent(agent.gameObject);
        if (target == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        navAgent.SetDestination(target.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, target.transform.position) <= 2)
        {
            state = BTNodeState.SUCCESS;

            navAgent.isStopped = true;

            enemy.AttackBT(target);

            return state;
        }
        else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }

    private SurvivorAgentUpdaterBT GetClosestAgent(GameObject hunter)
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        SurvivorAgentUpdaterBT closestAgent = null;
        float closestAgentDistance = 10000;
        foreach (GameObject ag in agents)
        {
            float distance = Vector3.Distance(ag.transform.position, hunter.transform.position);
            if (distance < closestAgentDistance)
            {
                closestAgentDistance = distance;
                SurvivorAgentUpdaterBT updater = ag.GetComponent<SurvivorAgentUpdaterBT>();
                if(updater != null) closestAgent = updater;
            }
        }

        return closestAgent;
    }
}

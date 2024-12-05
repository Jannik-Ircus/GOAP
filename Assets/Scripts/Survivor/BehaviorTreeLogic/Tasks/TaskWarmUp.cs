using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskWarmUp : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private GameObject firepit;

    public TaskWarmUp(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskWarmUp");
            return state;
        }

        if (firepit == null) firepit = agent.GetFirepit();
        if (firepit == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        if (Vector3.Distance(agent.transform.position, firepit.transform.position) <= 2)
        {
            state = BTNodeState.SUCCESS;
            navAgent.isStopped = true;
            return state;
        }

        navAgent.SetDestination(firepit.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, firepit.transform.position) <= 2)
        {
            state = BTNodeState.SUCCESS;
            navAgent.isStopped = true;
            return state;
        }
        else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

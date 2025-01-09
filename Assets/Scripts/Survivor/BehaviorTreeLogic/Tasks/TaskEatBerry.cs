using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskEatBerry : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private SurvivorBerry berry;

    public TaskEatBerry(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on EatBerry");
            return state;
        }

        if(berry == null) berry = agent.GetClosestBerry();
        if(berry == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        if(!berry.IsClaimed())berry.ClaimBerry(agent.gameObject);
        navAgent.SetDestination(berry.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, berry.transform.position) <= 2)
        {
            state = BTNodeState.SUCCESS;

            navAgent.isStopped = true;

            berry.DestroyBerry();
            agent.ModifyHunger(berry.foodValue);
            berry = null;
            return state;
        }
        else
        {
            state = BTNodeState.RUNNING;
            return state;
        }

    }
}

using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskBuildFirepit : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private SurvivorFirepit firepit;

    public TaskBuildFirepit(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskBuildFirepit");
            return state;
        }

        if(agent.currentWood < 1)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        if (firepit == null)
        {
            firepit = agent.GetFirepit().GetComponent<SurvivorFirepit>();
            if (firepit == null)
            {
                state = BTNodeState.FAILURE;
                Debug.LogError("Missing reference to firepit on TaskBuildFirepit");
                return state;
            }
        }

        navAgent.SetDestination(firepit.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, firepit.transform.position) <= 3)
        {
            state = BTNodeState.SUCCESS;

            navAgent.isStopped = true;

            agent.ModifyWood(-1);

            firepit.StartFire();

            return state;
        }
        else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

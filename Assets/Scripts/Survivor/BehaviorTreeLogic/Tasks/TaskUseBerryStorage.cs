using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskUseBerryStorage : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private SurvivorStorage storage;

    public TaskUseBerryStorage(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null ||agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskUseBerryStorage");
            return state;
        }
        if(storage == null) storage = agent.GetBerryStorage();
        if(storage == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing reference to BerryStorage on TaskUseBerryStorage");
            return state;
        }

        navAgent.SetDestination(storage.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, storage.transform.position) <= 3.5)
        {
            state = BTNodeState.SUCCESS;
            
            navAgent.isStopped = true;

            storage.ModifyStorage(-1);
            agent.ModifyHunger(5);

            return state;
        } else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

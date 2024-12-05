using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskStoreWood : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private SurvivorStorage storage;

    public TaskStoreWood(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null ||agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskUseWoodStorage");
            return state;
        }

        if(agent.currentWood < 1)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        if(storage == null) storage = agent.GetWoodStorage();
        if(storage == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing reference to WoodStorage on TaskUseBerryStorage");
            return state;
        }


        navAgent.SetDestination(storage.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if(Vector3.Distance(agent.transform.position, storage.transform.position) <= 3)
        {
            state = BTNodeState.SUCCESS;
            
            navAgent.isStopped = true;

            storage.ModifyStorage(1);

            agent.ModifyWood(-1);

            return state;
        } else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskUseWoodStorage : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private SurvivorStorage storage;
    private SurvivorFirepit firepit;

    public TaskUseWoodStorage(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
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

        if(storage == null) storage = agent.GetWoodStorage();
        if(storage == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing reference to WoodStorage on TaskUseBerryStorage");
            return state;
        }

        if(firepit == null)
        {
            firepit = agent.GetFirepit().GetComponent<SurvivorFirepit>();
            if(firepit == null)
            {
                state = BTNodeState.FAILURE;
                Debug.LogError("Missing reference to firepit on TaskUseBerryStorage");
                return state;
            }
        }

        navAgent.SetDestination(firepit.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if(Vector3.Distance(agent.transform.position, firepit.transform.position) <= 3.5)
        {
            state = BTNodeState.SUCCESS;
            
            navAgent.isStopped = true;

            storage.ModifyStorage(-1);

            firepit.StartFire();

            return state;
        } else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskPickUpWood : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private GameObject wood;

    public TaskPickUpWood(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskPickUpWood");
            return state;
        }

        if (wood == null) wood = agent.GetClosestWood();
        if (wood == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("No wood found on TaskPickUpWood");
            return state;
        }


        navAgent.SetDestination(wood.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, wood.transform.position) <= 3)
        {
            state = BTNodeState.SUCCESS;

            navAgent.isStopped = true;

            agent.PickUpWood(wood);
            wood = null;
            return state;
        }
        else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

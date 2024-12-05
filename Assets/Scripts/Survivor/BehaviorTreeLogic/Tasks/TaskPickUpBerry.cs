using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskPickUpBerry : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private GameObject berry;

    public TaskPickUpBerry(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskPickUpBerry");
            return state;
        }

        if (berry == null) berry = agent.GetClosestBerry().gameObject;
        if (berry == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("No wood found on TaskPickUpBerry");
            return state;
        }


        navAgent.SetDestination(berry.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, berry.transform.position) <= 3)
        {
            state = BTNodeState.SUCCESS;

            navAgent.isStopped = true;

            agent.PickUpBerry(berry);
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

using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoHome : BTNode
{
    private SurvivorEnemyBTUpdater agent;
    private NavMeshAgent navAgent;
    private GameObject home;

    public TaskGoHome(SurvivorEnemyBTUpdater agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on TaskGoHome");
            return state;
        }

        if (home == null) home = GetHome();
        if(home == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references to home on TaskGoHome");
            return state;
        }

        if (Vector3.Distance(agent.transform.position, home.transform.position) <= 2)
        {
            state = BTNodeState.SUCCESS;
            return state;
        }

        navAgent.SetDestination(home.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, home.transform.position) <= 2)
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

    private GameObject GetHome()
    {
        GameObject[] homes = GameObject.FindGameObjectsWithTag("EnemyBase");
        if (homes.Length <= 0)
        {
            Debug.LogError("No enemy base found!");
            return null;
        }

        return homes[0];
    }
}

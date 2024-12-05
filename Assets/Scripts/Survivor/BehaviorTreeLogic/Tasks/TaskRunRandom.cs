using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskRunRandom : BTNode
{
    private NavMeshAgent navAgent;
    private Vector3 randomPoint;
    private bool started = false;

   public TaskRunRandom(NavMeshAgent navAgent)
    {
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if(navAgent==null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("No NavMeshAgent found on TaskRunRandom");
            return state;
        }

        if(randomPoint == null ||randomPoint == Vector3.zero)
        {
            randomPoint = GetRandomPoint();
        }

        if(!started)
        {
            started = true;
            navAgent.SetDestination(randomPoint);
            navAgent.isStopped = false;
            navAgent.speed = 7;
        }

        if(navAgent.remainingDistance <= 5)
        {
            state = BTNodeState.SUCCESS;
            randomPoint = Vector3.zero;
            started = false;
            navAgent.isStopped = true;
            navAgent.speed = 3.5f;
            return state;
        } else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitCircle;

        randomPoint *= 50;
        randomPoint += navAgent.transform.position;

        Vector3 pointToReturn = Vector3.zero;
        Ray ray = new Ray(randomPoint, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            pointToReturn = hit.point;
        }

        return pointToReturn;
    }
}

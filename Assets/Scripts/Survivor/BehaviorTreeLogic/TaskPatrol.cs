using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPatrol : BTNode
{
    private Transform transform;
    private Transform[] waypoints;

    private int currentWaypointIndex = 0;

    private float waitTime = 1;
    private float waitCounter = 0;
    private bool waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        this.transform = transform;
        this.waypoints = waypoints;
    }

    public override BTNodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;
            if(waitCounter >= waitTime)
            {
                waiting = false;
            }
        }
        else
        {
            Transform wp = waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, wp.position) < 0.01f)
            {
                transform.position = wp.position;
                waitCounter = 0;
                waiting = true;

                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, wp.position, TestBT.speed * Time.deltaTime);
                transform.LookAt(wp.position);
            }
        }

        state = BTNodeState.RUNNING;
        return state;
    }
}

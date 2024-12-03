using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBT : BTTree
{
    public UnityEngine.Transform[] waypoints;
    public static float speed = 2;

    protected override BTNode SetupTree()
    {
        BTNode root = new TaskPatrol(transform, waypoints);

        return root;
    }
}

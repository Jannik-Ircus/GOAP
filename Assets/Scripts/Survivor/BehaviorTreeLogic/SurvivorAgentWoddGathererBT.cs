using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorAgentWoddGathererBT : BTTree
{

    public NavMeshAgent navAgent;

    protected override BTNode SetupTree()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if(navAgent == null)
        {
            Debug.LogError("No NavMeshAgent found on " + name);
            return null;
        }
        BTNode root = new TaskRunRandom(navAgent);

        return root;
    }
}

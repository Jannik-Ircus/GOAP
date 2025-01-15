using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorEnemyBT : BTTree
{
    public NavMeshAgent navAgent;
    public SurvivorEnemyBTUpdater agent;


    protected override BTNode SetupTree()
    {
        if (navAgent == null || agent == null)
        {
            Debug.LogError("Missing references on " + name);
            return null;
        }

        BTNode root = new BTSelector(new List<BTNode>
        {
            new BTSequence(new List<BTNode>
            {
                new CheckAgroValue(agent),
                new TaskHuntAgent(agent, navAgent)
            }),
            new TaskGoHome(agent, navAgent)
        });

        return root;
    }
}

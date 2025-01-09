using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskChopWood : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    private NavMeshAgent navAgent;
    private SurvivorTree tree;
    private bool currentlyChopping = false;

    public TaskChopWood(SurvivorAgentUpdaterBT agent, NavMeshAgent navAgent)
    {
        this.agent = agent;
        this.navAgent = navAgent;
    }

    public override BTNodeState Evaluate()
    {
        if (navAgent == null || agent == null)
        {
            state = BTNodeState.FAILURE;
            Debug.LogError("Missing references on EatBerry");
            return state;
        }

        if(tree==null)tree = GameObject.FindGameObjectWithTag("Tree").GetComponent<SurvivorTree>();
        if (tree == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }

        navAgent.SetDestination(tree.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = 3.5f;

        if (Vector3.Distance(agent.transform.position, tree.transform.position) <= 2)
        {
            

            navAgent.isStopped = true;

            SurvivorAnimationController animationController = agent.GetComponent<SurvivorAnimationController>();
            if (animationController != null) animationController.SetChoppingAnimation(true);

            tree.TreeAction();

            if(tree != null || (tree.choppingDuration - tree.progress) > 0)
            {
                tree = null;
                if (animationController != null) animationController.SetChoppingAnimation(false);

                state = BTNodeState.SUCCESS;
                return state;
            }

            state = BTNodeState.RUNNING;
            return state;

        }
        else
        {
            state = BTNodeState.RUNNING;
            return state;
        }
    }
}

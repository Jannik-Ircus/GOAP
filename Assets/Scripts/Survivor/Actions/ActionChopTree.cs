using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionChopTree : GOAPActionClass
{
    private SurvivorTree sTree;

    public override void AbortAction(GOAPAgent agent)
    {
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;

        SurvivorAnimationController animationController = agent.GetComponent<SurvivorAnimationController>();
        if (animationController != null) animationController.SetChoppingAnimation(false);

        if (sTree != null) sTree.PauseTreeAction();
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        if (trees.Length > 0) return true;
        else return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if (true)
        {
            SurvivorTree tree = GameObject.FindGameObjectWithTag(goalTag).GetComponent<SurvivorTree>();
            if (tree == null)
            {
                Debug.LogError("No firepit found for agent: " + agent.name);
                yield return null;
            }
            sTree = tree;

            if (Vector3.Distance(agent.gameObject.transform.position, tree.gameObject.transform.position) > 3)
            {
                NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
                if (navAgent == null)
                {
                    Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                    yield return new WaitForEndOfFrame();
                }

                navAgent.isStopped = false;
                navAgent.SetDestination(tree.gameObject.transform.position);

                yield return new WaitForSeconds(1);
                while (navAgent.remainingDistance >= navAgent.stoppingDistance + 2 && isRunning)
                {
                    yield return null;
                }

                navAgent.isStopped = true;
            }

            SurvivorAnimationController animationController = agent.GetComponent<SurvivorAnimationController>();
            if (animationController != null) animationController.SetChoppingAnimation(true);

            tree.TreeAction();


            yield return new WaitForSeconds(tree.choppingDuration);
            if (animationController != null) animationController.SetChoppingAnimation(false);
        }
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }
}

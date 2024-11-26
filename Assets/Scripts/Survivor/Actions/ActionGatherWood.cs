using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionGatherWood : GOAPActionClass
{
    protected GameObject nearestWood;
    protected virtual string resource { get; set; } = "Wood";
    public override void AbortAction(GOAPAgent agent)
    {
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;
    }

    public override float GetCost(GOAPAgent agent)
    {
        GameObject wood = GetClosestWoodObject(agent.gameObject);
        if (wood == null) return -1;
        nearestWood = wood;
        //Debug.Log("Distance to wood: " + Vector3.Distance(agent.gameObject.transform.position, wood.transform.position));
        return Vector3.Distance(agent.gameObject.transform.position, nearestWood.transform.position);
    }

    public override bool IsAchievable()
    {
        GameObject[] woodObjects = GameObject.FindGameObjectsWithTag(resource);
        if (woodObjects.Length <= 0) return false;
        else return true;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if(agent == null)
        {
            Debug.LogError("No agent found on " + this);
            yield return new WaitForEndOfFrame();
        }
        if(nearestWood == null)
        {
            nearestWood = GetClosestWoodObject(agent.gameObject);
            if (nearestWood != null)
            {
                NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
                if (navAgent == null)
                {
                    Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                    yield return new WaitForEndOfFrame();
                }

                navAgent.isStopped = false;
                navAgent.SetDestination(nearestWood.transform.position);

                yield return new WaitForSeconds(1);
                while (navAgent.remainingDistance >= navAgent.stoppingDistance && isRunning)
                {
                    yield return null;
                }

                navAgent.isStopped = true;
            }
        }
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
        if (nearestWood == null) return;
        nearestWood.GetComponent<SurvivorWood>().PickUpWood(agent);
    }

    public override void PrePerform(GOAPAgent agent)
    {

    }

    private GameObject GetClosestWoodObject(GameObject agentObject)
    {
        GameObject[] woodObjects = GameObject.FindGameObjectsWithTag(resource);
        if (woodObjects.Length <= 0)
        {
            Debug.LogError("No " + resource + " found in scene!");
            return null;
        }
        GameObject woodToReturn = woodObjects[0];
        float closestDistance = Vector3.Distance(agentObject.transform.position, woodToReturn.transform.position);
        foreach (GameObject obj in woodObjects)
        {
            //Debug.Log("Distance to wood: " + Vector3.Distance(agentObject.transform.position, obj.transform.position));
            if (Vector3.Distance(agentObject.gameObject.transform.position, obj.transform.position) < closestDistance)
            {
                woodToReturn = obj;
                closestDistance = Vector3.Distance(agentObject.gameObject.transform.position, obj.transform.position);
            }
        }
        return woodToReturn;
    }

}

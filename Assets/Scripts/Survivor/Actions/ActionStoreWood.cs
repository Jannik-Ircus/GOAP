using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionStoreWood : GOAPActionClass
{
    private string storageTag = "Storage";
    protected virtual string storedResource { get; set; } = "Wood";

    public override void AbortAction(GOAPAgent agent)
    {
        isRunning = false;
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.isStopped = true;
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable(GOAPAgent agent)
    {
        GameObject[] storage = GameObject.FindGameObjectsWithTag(storageTag);
        if (storage.Length == 0) return false;
        foreach(GameObject store in storage)
        {
            SurvivorStorage survivorStorage = store.GetComponent<SurvivorStorage>();
            if(survivorStorage.resource == storedResource)
            {
                if (survivorStorage.currentStorage < survivorStorage.maxStorage) return true;
            }
        }
        return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if (agent.agentStates.HasState(storedResource))
        {
            //Debug.Log(agent.gameObject + " starts building firepit...");
            GameObject[] storages = GameObject.FindGameObjectsWithTag(goalTag);
            SurvivorStorage storage = null;
            foreach (GameObject stora in storages)
            {
                SurvivorStorage tempStorage = stora.GetComponent<SurvivorStorage>();
                if (tempStorage.resource == storedResource) storage = tempStorage;
                
            }
            //
            if (storage == null)
            {
                Debug.LogError("No storage found for agent: " + agent.name);
                yield return null;
            }

            NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                yield return new WaitForEndOfFrame();
            }

            navAgent.isStopped = false;
            navAgent.SetDestination(storage.gameObject.transform.position);

            yield return new WaitForSeconds(1);
            while (navAgent.remainingDistance >= navAgent.stoppingDistance + 2 && isRunning)
            {
                yield return null;
            }

            navAgent.isStopped = true;

            storage.ModifyStorage(1);


            yield return new WaitForEndOfFrame();
        }
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
        if (!agent.agentStates.HasState(storedResource)) return;
        agent.agentStates.ModifyState(storedResource, -1);
        if (agent.agentStates.GetStates()[storedResource] <= 0) agent.agentStates.RemoveState(storedResource);
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }
}

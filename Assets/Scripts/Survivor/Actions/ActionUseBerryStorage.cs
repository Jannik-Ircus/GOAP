using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionUseBerryStorage : GOAPActionClass
{
    private string storageTag = "Storage";
    private string storageResource = "Berry";

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
        GameObject[] storages = GameObject.FindGameObjectsWithTag(storageTag);
        foreach (GameObject storage in storages)
        {
            SurvivorStorage survivorStorage = storage.GetComponent<SurvivorStorage>();
            if (survivorStorage == null) continue;
            if (survivorStorage.resource == storageResource && survivorStorage.currentStorage > 0) return true;
        }
        return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        
        GameObject[] storages = GameObject.FindGameObjectsWithTag(storageTag);
        SurvivorStorage storage = null;
        foreach (GameObject sto in storages)
        {
            SurvivorStorage survivorStorage = sto.GetComponent<SurvivorStorage>();
            if (survivorStorage == null) continue;
            if (survivorStorage.resource == storageResource) storage = survivorStorage;
        }
        //SurvivorStorage storage = GameObject.FindGameObjectWithTag(storageTag).GetComponent<SurvivorStorage>();
        if (storage == null)
        {
            Debug.LogError("No storage found for agent: " + agent.name);
            yield return null;
        }


        if (Vector3.Distance(agent.gameObject.transform.position, storage.gameObject.transform.position) > 3)
        {
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
        }


        storage.ModifyStorage(-1);
        if (agent.agentStates.HasState("hunger"))
        {
            agent.agentStates.ModifyState("hunger", 5);
        }



        yield return new WaitForEndOfFrame();
        
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }
}

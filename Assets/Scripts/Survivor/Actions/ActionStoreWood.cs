using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionStoreWood : GOAPActionClass
{
    private string storageTag = "Storage";
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

    public override bool IsAchievable()
    {
        GameObject storage = GameObject.FindGameObjectWithTag(storageTag);
        if (storage == null) return false;
        SurvivorStorage survivorStorage = storage.GetComponent<SurvivorStorage>();
        if (survivorStorage == null) return false;
        if (survivorStorage.currentStorage < survivorStorage.maxStorage) return true;
        else return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if (agent.agentStates.HasState("Wood"))
        {
            //Debug.Log(agent.gameObject + " starts building firepit...");
            SurvivorStorage storage = GameObject.FindGameObjectWithTag(goalTag).GetComponent<SurvivorStorage>();
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
        if (!agent.agentStates.HasState("Wood")) return;
        agent.agentStates.ModifyState("Wood", -1);
        if (agent.agentStates.GetStates()["Wood"] <= 0) agent.agentStates.RemoveState("Wood");
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }
}

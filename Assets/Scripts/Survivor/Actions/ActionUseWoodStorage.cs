using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionUseWoodStorage : GOAPActionClass
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
        if (survivorStorage.currentStorage > 0) return true;
        else return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if (GOAPWorld.Instance.GetWorld().GetStateValue("firepit") == 0)
        {
            SurvivorFirepit firepit = GameObject.FindGameObjectWithTag(goalTag).GetComponent<SurvivorFirepit>();
            if (firepit == null)
            {
                Debug.LogError("No firepit found for agent: " + agent.name);
                yield return null;
            }

            SurvivorStorage storage = GameObject.FindGameObjectWithTag(storageTag).GetComponent<SurvivorStorage>();
            if (storage == null)
            {
                Debug.LogError("No storage found for agent: " + agent.name);
                yield return null;
            }


            if(Vector3.Distance(agent.gameObject.transform.position, firepit.gameObject.transform.position) < 3)
            {
                NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
                if (navAgent == null)
                {
                    Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                    yield return new WaitForEndOfFrame();
                }

                navAgent.isStopped = false;
                navAgent.SetDestination(firepit.gameObject.transform.position);

                yield return new WaitForSeconds(1);
                while (navAgent.remainingDistance >= navAgent.stoppingDistance + 2 && isRunning)
                {
                    yield return null;
                }

                navAgent.isStopped = true;
            }
            

            if (GOAPWorld.Instance.GetWorld().GetStateValue("firepit") == 0)
            {
                storage.ModifyStorage(-1);
                firepit.StartFire();
            }


            yield return new WaitForEndOfFrame();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionBuildFirepit : GOAPActionClass
{
    private bool woodUsed = false;
    public override void AbortAction(GOAPAgent agent)
    {
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable()
    {
        //return GOAPWorld.Instance.GetWorld().HasState("firepit");
        return true;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        isRunning = true;
        if(agent.agentStates.HasState("Wood") && GOAPWorld.Instance.GetWorld().GetStateValue("firepit")==0)
        {
            Debug.Log(agent.gameObject + " starts building firepit...");
            SurvivorFirepit firepit = GameObject.FindGameObjectWithTag(goalTag).GetComponent<SurvivorFirepit>();
            if(firepit==null)
            {
                Debug.LogError("No firepit found for agent: " + agent.name);
                yield return null;
            }

            NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                yield return new WaitForEndOfFrame();
            }

            navAgent.isStopped = false;
            navAgent.SetDestination(firepit.gameObject.transform.position);

            yield return new WaitForSeconds(1);
            while (navAgent.remainingDistance >= navAgent.stoppingDistance+2 && isRunning)
            {
                yield return null;
            }

            navAgent.isStopped = true;

            if(GOAPWorld.Instance.GetWorld().GetStateValue("firepit") == 0)
            {
                woodUsed = true;
                firepit.StartFire();
            }
            
            
            yield return new WaitForSeconds(2f);
        }
        isRunning = false;
    }

    public override void PostPerform(GOAPAgent agent)
    {
        if (!agent.agentStates.HasState("Wood")) return;
        if(woodUsed)agent.agentStates.ModifyState("Wood", -1);
        if (agent.agentStates.GetStates()["Wood"] <= 0) agent.agentStates.RemoveState("Wood");
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }
}

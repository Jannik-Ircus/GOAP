using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return !GOAPWorld.Instance.GetWorld().HasState("firepit");
        //return true;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        if(agent.agentStates.HasState("Wood") && !GOAPWorld.Instance.GetWorld().HasState("firepit"))
        {
            Debug.Log(agent.gameObject + " starts building firepit...");
            

            SurvivorAgent survivor = agent.GetComponent<SurvivorAgent>();
            if (survivor != null)
            {
                survivor.BuildFirepit();
                woodUsed = true;
            }
            else
            {
                Debug.LogError("Failed to find SurvivorAgent on " + agent.gameObject);
            }
            yield return new WaitForSeconds(2f);
        }
        
    }

    public override void PostPerform(GOAPAgent agent)
    {
        if (!agent.agentStates.HasState("Wood")) return;
        if(woodUsed)agent.agentStates.ModifyState("Wood", -1);
        if (agent.agentStates.GetStates()["Wood"] <= 0) agent.agentStates.RemoveState("Wood");
        //agent.agentStates.AddState("firepit", 1);
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }
}

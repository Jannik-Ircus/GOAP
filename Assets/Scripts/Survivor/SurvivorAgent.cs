using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAgent : GOAPAgentStateUpdater
{
    private bool hasFirepitInWorld = true;
    private string firepitTag = "firepit";
    private string warmthTag = "warmth";

    public override void StartAgentStates(GOAPAgent agent)
    {
        if (GOAPWorld.Instance.GetWorld().HasState("firepit") && !agent.agentStates.HasState(firepitTag)) agent.agentStates.AddState(firepitTag, 0);
        else if (!GOAPWorld.Instance.GetWorld().HasState("firepit")) hasFirepitInWorld = false;
        agent.agentStates.AddState(warmthTag, 10);
        StartCoroutine(LoseWarmth(agent));
    }
    public override void UpdateAgentStates(GOAPAgent agent)
    {
        if(hasFirepitInWorld)
        {
            int firepitValue = GOAPWorld.Instance.GetWorld().GetStateValue(firepitTag);
            if(agent.agentStates.GetStateValue(firepitTag) != firepitValue)agent.agentStates.SetState(firepitTag, firepitValue);
        }
        
    }

    private IEnumerator LoseWarmth(GOAPAgent agent)
    {
        yield return new WaitForSeconds(1);
        if(agent.agentStates.GetStateValue(warmthTag) > 0)
        {
            agent.agentStates.ModifyState(warmthTag, -1);
        }
        StartCoroutine(LoseWarmth(agent));
    }
    
}

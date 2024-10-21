using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAgent : GOAPAgentStateUpdater
{
    private bool hasFirepitInWorld = true;
    public override void StartAgentStates(GOAPAgent agent)
    {
        if (GOAPWorld.Instance.GetWorld().HasState("firepit") && !agent.agentStates.HasState("firepit")) agent.agentStates.AddState("firepit", 0);
        else if (!GOAPWorld.Instance.GetWorld().HasState("firepit")) hasFirepitInWorld = false;
    }
    public override void UpdateAgentStates(GOAPAgent agent)
    {
        if(hasFirepitInWorld)
        {
            int firepitValue = GOAPWorld.Instance.GetWorld().GetStateValue("firepit");
            if(agent.agentStates.GetStateValue("firepit") != firepitValue)agent.agentStates.SetState("firepit", firepitValue);
        }
        
    }

    
}

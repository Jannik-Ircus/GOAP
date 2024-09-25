using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAgent : GOAPAgentStateUpdater
{
    public GameObject firepit;
    public void BuildFirepit()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        Instantiate(firepit, spawnPosition, transform.rotation);

    }

    public override void UpdateAgentStates(GOAPAgent agent)
    {
        if (GOAPWorld.Instance.GetWorld().HasState("firepit") && !agent.agentStates.HasState("firepit")) agent.agentStates.AddState("firepit", 1);
        else if (!GOAPWorld.Instance.GetWorld().HasState("firepit") && agent.agentStates.HasState("firepit")) agent.agentStates.RemoveState("firepit");
    }
}

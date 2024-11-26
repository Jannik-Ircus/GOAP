using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorWood : SurvivorResource
{
    SurvivorWood()
    {
        resourceTag = "Wood";
    }
    /*public void PickUpWood(GOAPAgent agent)
    {
        if(!agent.agentStates.HasState("Wood"))
        {
            agent.agentStates.AddState("Wood", 1);
        } else
        {
            agent.agentStates.SetState("Wood", 1);
        }

        Destroy(this.gameObject);
    }*/
}

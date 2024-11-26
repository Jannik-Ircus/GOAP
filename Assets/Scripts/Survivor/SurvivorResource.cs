using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorResource : MonoBehaviour
{
    public bool isStored = false;
    protected  string resourceTag;

    public void PickUpResource(GOAPAgent agent)
    {
        if (!agent.agentStates.HasState(resourceTag))
        {
            agent.agentStates.AddState(resourceTag, 1);
        }
        else
        {
            agent.agentStates.SetState(resourceTag, 1);
        }

        Destroy(this.gameObject);
    }
}

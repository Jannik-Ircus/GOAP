using UnityEngine;

public class SurvivorBerry : SurvivorResource
{
    public bool claimed = false;
    public int foodValue = 3;
    private string hungerTag = "hunger";
    //private string berryTag = "Berry";

    SurvivorBerry()
    {
        resourceTag = "Berry";
    }

    public void EatBerry(GOAPAgent agent)
    {
        if (agent.agentStates.HasState(hungerTag))
        {
            agent.agentStates.ModifyState(hungerTag, foodValue);
        }

        Destroy(this.gameObject);
    }

    /*public void PickUpBerry(GOAPAgent agent)
    {
        if (!agent.agentStates.HasState(berryTag))
        {
            agent.agentStates.AddState(berryTag, 1);
        }
        else
        {
            agent.agentStates.SetState(berryTag, 1);
        }

        Destroy(this.gameObject);
    }*/
}

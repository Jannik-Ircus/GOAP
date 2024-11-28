using System.Collections;
using UnityEngine;

public class SurvivorBerry : SurvivorResource
{
    private GameObject claimedBy = null;
    public int foodValue = 3;
    private string hungerTag = "hunger";
    //private string berryTag = "Berry";

    SurvivorBerry()
    {
        resourceTag = "Berry";
    }

    private void Start()
    {
        claimedBy = null;
    }

    public void EatBerry(GOAPAgent agent)
    {
        if (agent.agentStates.HasState(hungerTag))
        {
            agent.agentStates.ModifyState(hungerTag, foodValue);
        }

        if(this!=null)Destroy(this.gameObject);
    }

    public void ClaimBerry(GameObject agent)
    {
        if (claimedBy == null)
        {
            claimedBy = agent;
            StartCoroutine(UnclaimBerry());
        }
    }

    private IEnumerator UnclaimBerry()
    {
        yield return new WaitForSeconds(10);
        claimedBy = null;
    }

    public bool IsClaimed(GameObject agentToCheck)
    {
        if (claimedBy == null) return false;
        if (agentToCheck == claimedBy) return false;
        else return true;
    }

    public bool IsClaimed()
    {
        if (claimedBy == null) return false;
        else return true;
    }
}

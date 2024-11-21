using UnityEngine;

public class SurvivorBerry : MonoBehaviour
{
    public bool claimed = false;
    public int foodValue = 3;
    private string hungerTag = "hunger";

    public void EatBerry(GOAPAgent agent)
    {
        if (agent.agentStates.HasState(hungerTag))
        {
            agent.agentStates.ModifyState(hungerTag, foodValue);
        }

        Destroy(this.gameObject);
    }
}

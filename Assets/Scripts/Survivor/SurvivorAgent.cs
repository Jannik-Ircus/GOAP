using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAgent : GOAPAgentStateUpdater
{
    private bool hasFirepitInWorld = true;
    private string firepitTag = "firepit";
    private string warmthTag = "warmth";
    private string healthTag = "health";
    public float warmthDecreaseTime = 1;

    public ProgressBar healthBar;
    public ProgressBar tempBar;

    public override void StartAgentStates(GOAPAgent agent)
    {
        if (GOAPWorld.Instance.GetWorld().HasState("firepit") && !agent.agentStates.HasState(firepitTag)) agent.agentStates.AddState(firepitTag, 0);
        else if (!GOAPWorld.Instance.GetWorld().HasState("firepit")) hasFirepitInWorld = false;
        agent.agentStates.AddState(warmthTag, 10);
        StartCoroutine(LoseWarmth(agent));

        if (healthBar == null && tempBar == null) Debug.LogError("Missing reference to health or temperature bar");
    }
    public override void UpdateAgentStates(GOAPAgent agent)
    {
        if(hasFirepitInWorld)
        {
            int firepitValue = GOAPWorld.Instance.GetWorld().GetStateValue(firepitTag);
            if(agent.agentStates.GetStateValue(firepitTag) != firepitValue)agent.agentStates.SetState(firepitTag, firepitValue);
        }

        //healthBar.SetProgressBar((float)agent.agentStates.GetStateValue(healthTag) /10f);
        tempBar.SetProgressBar((float)agent.agentStates.GetStateValue(warmthTag) /10f);
    }

    private IEnumerator LoseWarmth(GOAPAgent agent)
    {
        yield return new WaitForSeconds(warmthDecreaseTime);
        if(agent.agentStates.GetStateValue(warmthTag) > 0)
        {
            agent.agentStates.ModifyState(warmthTag, -1);
        }
        StartCoroutine(LoseWarmth(agent));
    }
    
}

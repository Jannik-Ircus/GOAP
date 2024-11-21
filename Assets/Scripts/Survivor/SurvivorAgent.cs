using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAgent : GOAPAgentStateUpdater
{
    private bool hasFirepitInWorld = true;
    private string firepitTag = "firepit";
    private string warmthTag = "warmth";
    private string healthTag = "health";
    private string hungerTag = "hunger";

    public int maxWarmth = 10;
    public float warmthDecreaseTime = 1;

    
    public int maxHunger = 10;
    public float hungerDecreaseTime = 1;

    public int maxHealth = 10;

    public ProgressBar healthBar;
    public ProgressBar tempBar;
    public ProgressBar hungerBar;

    public override void StartAgentStates(GOAPAgent agent)
    {
        if (GOAPWorld.Instance.GetWorld().HasState("firepit") && !agent.agentStates.HasState(firepitTag)) agent.agentStates.AddState(firepitTag, 0);
        else if (!GOAPWorld.Instance.GetWorld().HasState("firepit")) hasFirepitInWorld = false;
        agent.agentStates.AddState(warmthTag, maxWarmth);
        agent.agentStates.AddState(hungerTag, maxHunger);
        agent.agentStates.AddState(healthTag, maxHealth);
        StartCoroutine(LoseWarmth(agent));
        StartCoroutine(LoseHunger(agent));

        if (healthBar == null && tempBar == null) Debug.LogError("Missing reference to health or temperature bar");
    }
    public override void UpdateAgentStates(GOAPAgent agent)
    {
        if(hasFirepitInWorld)
        {
            int firepitValue = GOAPWorld.Instance.GetWorld().GetStateValue(firepitTag);
            if(agent.agentStates.GetStateValue(firepitTag) != firepitValue)agent.agentStates.SetState(firepitTag, firepitValue);
        }

        healthBar.SetProgressBar((float)agent.agentStates.GetStateValue(healthTag) /10f);
        tempBar.SetProgressBar((float)agent.agentStates.GetStateValue(warmthTag) /10f);
        hungerBar.SetProgressBar((float)agent.agentStates.GetStateValue(hungerTag) / 10f);

        SetPriorities(agent);
    }

    private IEnumerator LoseWarmth(GOAPAgent agent)
    {
        yield return new WaitForSeconds(warmthDecreaseTime);
        if(agent.agentStates.GetStateValue(warmthTag) > 0)
        {
            agent.agentStates.ModifyState(warmthTag, -1);
        }
        else if (agent.agentStates.GetStateValue(warmthTag) == 0)
        {
            LoseHealth(agent);
        }
        StartCoroutine(LoseWarmth(agent));
    }

    private IEnumerator LoseHunger(GOAPAgent agent)
    {
        yield return new WaitForSeconds(hungerDecreaseTime);
        if(agent.agentStates.GetStateValue(hungerTag) > 0)
        {
            agent.agentStates.ModifyState(hungerTag, -1);
        } 
        else if(agent.agentStates.GetStateValue(hungerTag) == 0)
        {
            LoseHealth(agent);
        }
        StartCoroutine(LoseHunger(agent));
    }

    private void LoseHealth(GOAPAgent agent)
    {
        agent.agentStates.ModifyState(healthTag, -1);
        if(agent.agentStates.GetStateValue(healthTag) <= 0)
        {
            Debug.Log("Agent: " + gameObject.name + " has no health and died!");
            Destroy(this.gameObject);
        }
    }

    private void SetPriorities(GOAPAgent agent)
    {
        agent.SetGoalPriority(warmthTag, maxWarmth - agent.agentStates.GetStateValue(warmthTag));
        agent.SetGoalPriority(hungerTag, maxHunger - agent.agentStates.GetStateValue(hungerTag) +1);

    }
    
}

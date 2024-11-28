using System.Collections;
using UnityEngine;

public class SurvivorEnemyUpdater : GOAPAgentStateUpdater
{
    public Animator animator;
    private int attackHash;
    private string agroTag = "agro";
    [Range(0.1f, 5)]
    public float agroRate = 3;
    [Range(1f, 15)]
    public int calmDownRate = 5;

    private bool hunting = false;

    public override void StartAgentStates(GOAPAgent agent)
    {
        attackHash = Animator.StringToHash("Attack");
        if (!agent.agentStates.HasState(agroTag)) agent.agentStates.AddState(agroTag, 0);
        StartCoroutine(GetAgro(agent));

    }

    public override void UpdateAgentStates(GOAPAgent agent)
    {
        SetPriorities(agent);   
    }

    public void Attack(GOAPAgent agent, GOAPAgent hunter)
    {
        if (agent == null)
        {
            Debug.LogError("No agent found while attacking with: " + hunter.name);
            return;
        }
        PlayAttackAnimation();

        if(agent.agentStates.HasState("health"))
        {
            agent.agentStates.ModifyState("health", -1);
            if (agent.agentStates.HasState("spottedEnemy")) agent.agentStates.SetState("spottedEnemy", 1);
            StartCoroutine(CalmDownAgent(agent));
        }

        if (hunter.agentStates.HasState(agroTag)) hunter.agentStates.ModifyState(agroTag, -3);
        hunting = false;
    }

    private void PlayAttackAnimation()
    {
        animator.SetTrigger(attackHash);
    }

    private void SetPriorities(GOAPAgent agent)
    {
        agent.SetGoalPriority(agroTag, agent.agentStates.GetStateValue(agroTag));
    }

    private IEnumerator GetAgro(GOAPAgent agent)
    {
        if(agent.agentStates.GetStateValue(agroTag)<10)
        {
            agent.agentStates.ModifyState(agroTag, 1);
        }
        yield return new WaitForSeconds(agroRate);

        StartCoroutine(GetAgro(agent));
    }

    public void StartHunt(GOAPAgent agent)
    {
        StartCoroutine(CalmDown(agent));
        hunting = true;
    }

    private IEnumerator CalmDown(GOAPAgent agent)
    {
        yield return new WaitForSeconds(calmDownRate);

        if (agent.agentStates.GetStateValue(agroTag) < 10)
        {
            if(!hunting)agent.agentStates.SetState(agroTag, 0);
            hunting = false;
        }
    }

    private IEnumerator CalmDownAgent(GOAPAgent agent)
    {
        yield return new WaitForSeconds(6);
        if (agent.agentStates.HasState("spottedEnemy")) agent.agentStates.SetState("spottedEnemy", 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentUpdaterTest : GOAPAgentStateUpdater
{
    public string goalTag = "goal";
    private GameObject goal;
    public string spawnTag = "spawn";
    private GameObject spawn;
    public string stateToAdd;

    private void Awake()
    {
        goal = GameObject.FindGameObjectWithTag(goalTag);
        spawn = GameObject.FindGameObjectWithTag(spawnTag);
    }
    public override void UpdateAgentStates(GOAPAgent agent)
    {
        if (goal == null) return;
        if(Vector3.Distance(agent.gameObject.transform.position, goal.transform.position) < 1.5) agent.agentStates.AddState(stateToAdd, 0);
        if (spawn == null) return;
        if (Vector3.Distance(agent.gameObject.transform.position, spawn.transform.position) < 2.5) agent.agentStates.RemoveState(stateToAdd);
    }

    public override void StartAgentStates(GOAPAgent agent)
    {
        
    }
}

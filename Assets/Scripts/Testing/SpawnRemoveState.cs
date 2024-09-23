using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRemoveState : MonoBehaviour
{
    public GameObject agent;
    public string stateToRemove;

    private void Update()
    {
        //Debug.Log("Distance: " + Vector3.Distance(transform.position, agent.transform.position));
        if (Vector3.Distance(transform.position, agent.transform.position) < 1.5)
        {
            GOAPAgent goapAgent = agent.GetComponent<GOAPAgent>();
            if (goapAgent == null)
            {
                Debug.LogError("No GOAPAgent found");
                Destroy(this);
            }
            if (goapAgent.agentStates.HasState(stateToRemove)) goapAgent.agentStates.RemoveState(stateToRemove);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAddState : MonoBehaviour
{
    public GameObject agent;
    public string stateToAdd;

    private void Update()
    {
        //Debug.Log("Distance: " + Vector3.Distance(transform.position, agent.transform.position));
        if(Vector3.Distance(transform.position, agent.transform.position) < 1.5)
        {
            GOAPAgent goapAgent = agent.GetComponent<GOAPAgent>();
            if(goapAgent == null)
            {
                Debug.LogError("No GOAPAgent found");
                Destroy(this);
            }
            if(!goapAgent.agentStates.HasState(stateToAdd)) goapAgent.agentStates.AddState(stateToAdd, 0);
        }
    }
}

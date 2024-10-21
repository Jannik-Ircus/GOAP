using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorInventory : MonoBehaviour
{
    public GameObject wood;
    private GOAPAgent agent;
    private void Start()
    {
        agent = transform.parent.gameObject.GetComponent<GOAPAgent>();
        if(agent==null)
        {
            Debug.LogError("No agent found on: " + name);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if(agent.agentStates.HasState("Wood"))
        {
            wood.SetActive(true);
        } else
        {
            wood.SetActive(false);
        }
    }
}

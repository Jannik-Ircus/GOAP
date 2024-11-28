using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorInventory : MonoBehaviour
{
    public GameObject wood;
    public GameObject berry;
    private GOAPAgent agent;
    private bool hasWood = false;
    private bool hasBerry = false;
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
            hasWood = true;
        } else
        {
            wood.SetActive(false);
            hasWood = false;
        }
        if (agent.agentStates.HasState("Berry"))
        {
            berry.SetActive(true);
            hasBerry = true;
        }
        else
        {
            berry.SetActive(false);
            hasBerry = false;
        }

        if(hasWood && hasBerry)
        {
            berry.transform.localPosition = new Vector3(0, 0.65f, -0.013f);
        } else
        {
            berry.transform.localPosition = new Vector3(0, 0, -0.013f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject agentToSpawn;
    public int numberOfAgents;
    public GOAPPlanner planner;

    private void Start()
    {
        if(agentToSpawn == null ||planner == null)
        {
            Debug.LogError("Missing reference on " + gameObject.name);
            Destroy(this);
        }
        if(agentToSpawn.GetComponent<GOAPAgent>() == null)
        {
            Debug.LogError("Missing GOAPAgent on " + agentToSpawn.name);
            Destroy(this);
        }

        List<GOAPAgent> agents = new List<GOAPAgent>();
        for (int i = 0; i <= numberOfAgents; i++)
        {
            GameObject agent = Instantiate(agentToSpawn, transform);
            agents.Add(agent.GetComponent<GOAPAgent>());
        }

        planner.SetAgents(agents);
    }
}

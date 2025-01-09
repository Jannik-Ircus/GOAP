using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerItem
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 1;
    public bool isGoap;

    public SpawnerItem(GameObject objectToSpawn, int numberOfObjects)
    {
        this.objectToSpawn = objectToSpawn;
        this.numberOfObjects = numberOfObjects;
    }

    public SpawnerItem(GameObject objectToSpawn, int numberOfObjects, bool isGoap)
    {
        this.objectToSpawn = objectToSpawn;
        this.numberOfObjects = numberOfObjects;
        this.isGoap = isGoap;
    }
}

public class Spawner : MonoBehaviour
{
    //public GameObject agentToSpawn;
    //public int numberOfAgents;
    public List<SpawnerItem> agentsToSpawn;
    public GOAPPlanner planner;

    private void Start()
    {
        if(agentsToSpawn.Count == 0 ||planner == null)
        {
            Debug.LogError("Missing reference on " + gameObject.name);
            Destroy(this);
        }
        /*if(agentToSpawn.GetComponent<GOAPAgent>() == null)
        {
            Debug.LogError("Missing GOAPAgent on " + agentToSpawn.name);
            Destroy(this);
        }*/

        /*List<GOAPAgent> agents = new List<GOAPAgent>();
        for (int i = 0; i <= numberOfAgents-1; i++)
        {
            GameObject agent = Instantiate(agentToSpawn, transform);
            agents.Add(agent.GetComponent<GOAPAgent>());
        }*/
        List<GOAPAgent> agents = new List<GOAPAgent>();
        foreach (SpawnerItem item in agentsToSpawn)
        {
            for(int i = 0; i <= item.numberOfObjects - 1; i++)
            {
                GameObject agent = Instantiate(item.objectToSpawn, transform);
                agents.Add(agent.GetComponent<GOAPAgent>());
            }
        }

        planner.SetAgents(agents);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private List<SpawnerItem> spawnerList = new List<SpawnerItem>();
    public Transform spawnPoint;
    public GOAPPlanner planner;

    private void Start()
    {
        if(spawnPoint == null || planner == null)
        {
            Debug.LogError("Missing references on " + gameObject.name);
            Destroy(this.gameObject);
        }
    }

    public void AddToList(GameObject objectToSpawn, int number, bool isGoap)
    {

        foreach(SpawnerItem item in spawnerList)
        {
            if(item.objectToSpawn == objectToSpawn)
            {
                Debug.Log("Object: " + objectToSpawn.name + " is already part of spawnList");
                return;
            }
        }
        SpawnerItem newItem = new SpawnerItem(objectToSpawn, number, isGoap);
        spawnerList.Add(newItem);
    }

    public void ModifyNumber(GameObject objectToSpawn, int number)
    {
        foreach (SpawnerItem item in spawnerList)
        {
            if (item.objectToSpawn == objectToSpawn)
            {
                item.numberOfObjects = number;
            }
        }
    }

    public void SpawnAgents()
    {
        List<GOAPAgent> agents = new List<GOAPAgent>();
        foreach (SpawnerItem item in spawnerList)
        {
            for (int i = 0; i <= item.numberOfObjects - 1; i++)
            {
                GameObject agent = Instantiate(item.objectToSpawn, spawnPoint);
                if(item.isGoap)agents.Add(agent.GetComponent<GOAPAgent>());
            }
        }

        planner.SetAgents(agents);

        planner.StartPlanner();
    }

    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }
}

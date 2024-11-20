using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorBerryBush : MonoBehaviour
{
    private GameObject berry;
    public GameObject berryHolder;
    public GameObject berryToSpawn;

    [Range(0, 10)] public int spawnTimerMin = 1;
    [Range(3, 30)] public int spawnTimerMax = 5;

    private bool spawningBerry = false;

    private void Start()
    {
        if(berryHolder == null || berryToSpawn == null)
        {
            Debug.Log("Missing references on " + this.gameObject.name);
            Destroy(this);
        }
        berry = Instantiate(berryToSpawn, berryHolder.transform);
    }

    private void Update()
    {
        if(berry==null && !spawningBerry)
        {
            spawningBerry = true;
            StartCoroutine(SpawnBerry());
        }
    }

    private IEnumerator SpawnBerry()
    {
        if (spawnTimerMax < spawnTimerMin) spawnTimerMin = 0;
        float timer = Random.Range(spawnTimerMin, spawnTimerMax);

        yield return new WaitForSeconds(timer);

        berry = Instantiate(berryToSpawn, berryHolder.transform);
        spawningBerry = false;
    }

}

using System.Collections;
using UnityEngine;

public class SurvivorAgentUpdaterBT : MonoBehaviour
{
    public SurvivorEnemySpotterBT enemySpotter;
    private SurvivorStorage berryStorage;

    [Header("Stats")]
    public int health = 10;
    public int hunger = 10;
    public int warmth = 10;

    [Header("Settings")]
    [Range(1, 10)]
    public int hungerMinimum = 5;

    [Range(0, 10)]
    public float hungerDecreaseDelay = 3;

    private void Start()
    {

        SetStorages();

        if(enemySpotter == null || berryStorage == null)
        {
            Debug.LogError("Missing references on " + name);
            Destroy(this);
        }



        StartCoroutine(DecreaseHunger());
    }

    public bool GetEnemySpotted()
    {
        return enemySpotter.GetSpottedEnemy();
    }

    public bool GetIsHungry()
    {
        if (GetClosestBerry() == null) return false;

        if(hunger <= warmth)
        {
            if (hunger < hungerMinimum) return true;
        }

        return false;
    }

    public SurvivorBerry GetClosestBerry()
    {
        GameObject[] berries = GameObject.FindGameObjectsWithTag("Berry");
        SurvivorBerry berryToReturn = null;
        float closestBerry = 1000;

        foreach(GameObject berry in berries)
        {
            if(Vector3.Distance(transform.position, berry.transform.position) < closestBerry)
            {
                SurvivorBerry newBerry = berry.GetComponent<SurvivorBerry>();
                if(newBerry != null)
                {
                    if (newBerry.IsClaimed() || newBerry.isStored) continue;
                    berryToReturn = newBerry;
                    closestBerry = Vector3.Distance(transform.position, berry.transform.position);
                }
            }
        }

        return berryToReturn;
    }

    public int GetBerryStorageAmount()
    {
        return berryStorage.currentStorage;
    }

    public SurvivorStorage GetBerryStorage()
    {
        return berryStorage;
    }

    public void ModifyHunger(int value)
    {
        hunger = Mathf.Clamp(hunger + value, 0, 10);
    }

    private IEnumerator DecreaseHunger()
    {
        yield return new WaitForSeconds(hungerDecreaseDelay);

        if (hunger > 0) hunger--;

        StartCoroutine(DecreaseHunger());
    }

    private void SetStorages()
    {
        GameObject[] storages = GameObject.FindGameObjectsWithTag("Storage");
        foreach(GameObject storage in storages)
        {
            SurvivorStorage st = storage.GetComponent<SurvivorStorage>();
            if(st != null)
            {
                if (st.resource == "Berry") berryStorage = st;
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class SurvivorAgentUpdaterBT : MonoBehaviour
{
    public SurvivorEnemySpotterBT enemySpotter;
    private SurvivorStorage berryStorage;
    private SurvivorFirepit firepit;
    private SurvivorStorage woodStorage;

    [Header("Stats")]
    public int health = 10;
    public int hunger = 10;
    public int warmth = 10;
    public int currentWood = 0;

    [Header("Settings")]
    [Range(1, 10)]
    public int hungerMinimum = 5;

    [Range(0, 10)]
    public float hungerDecreaseDelay = 3;

    [Range(1, 10)]
    public int warmthMinimum = 5;

    [Range(0, 10)]
    public float warmthDecreaseDelay = 3;

    private void Start()
    {

        SetStorages();

        if(enemySpotter == null || berryStorage == null ||firepit == null ||woodStorage == null)
        {
            Debug.LogError("Missing references on " + name);
            Destroy(this);
        }



        StartCoroutine(DecreaseHunger());
        StartCoroutine(DecreaseWarmth());
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

    public bool GetIsCold()
    {
        if (GetClosestWood() == null) return false;

        if (warmth < hunger)
        {
            if (warmth < warmthMinimum) return true;
        }

        return false;
    }

    public GameObject GetFirepit()
    {
        return firepit.gameObject;
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

    public GameObject GetClosestWood()
    {
        GameObject[] woods = GameObject.FindGameObjectsWithTag("Wood");
        GameObject woodToReturn = null;
        float closestWood = 1000;

        foreach (GameObject wood in woods)
        {
            if (Vector3.Distance(transform.position, wood.transform.position) < closestWood)
            {
                
                woodToReturn = wood;
                closestWood = Vector3.Distance(transform.position, wood.transform.position);
                
            }
        }

        return woodToReturn;
    }

    public int GetBerryStorageAmount()
    {
        return berryStorage.currentStorage;
    }

    public SurvivorStorage GetBerryStorage()
    {
        return berryStorage;
    }

    public int GetWoodStorageAmount()
    {
        return woodStorage.currentStorage;
    }

    public SurvivorStorage GetWoodStorage()
    {
        return woodStorage;
    }

    public bool IsFirepitActive()
    {
        bool isBurning = firepit.FireIsActive();
        return isBurning;
    }

    public void ModifyHunger(int value)
    {
        hunger = Mathf.Clamp(hunger + value, 0, 10);
    }

    public void ModifyWarmth(int value)
    {
        warmth = Mathf.Clamp(warmth + value, 0, 10);
    }

    public void ModifyWood(int value)
    {
        currentWood = Mathf.Clamp(currentWood + value, 0, 10);
    }

    public void PickUpWood(GameObject wood)
    {
        currentWood = Mathf.Clamp(currentWood + 1, 0, 10);
        Destroy(wood);
    }

    private IEnumerator DecreaseHunger()
    {
        yield return new WaitForSeconds(hungerDecreaseDelay);

        if (hunger > 0) hunger--;

        StartCoroutine(DecreaseHunger());
    }

    private IEnumerator DecreaseWarmth()
    {
        yield return new WaitForSeconds(warmthDecreaseDelay);

        if (warmth > 0) warmth--;

        StartCoroutine(DecreaseWarmth());
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
                else if (st.resource == "Wood") woodStorage = st;
            }
        }

        GameObject fire = GameObject.FindGameObjectWithTag("Firepit");
        if(fire == null)
        {
            Debug.LogError("No firepit found");
            return;
        }
        firepit = fire.GetComponent<SurvivorFirepit>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorEnemySpotterBT : MonoBehaviour
{
    [Range(1, 15)]
    public int calmDownTime = 5;
    private bool spottedEnemy = false;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SurvivorEnemyUpdater>() && !spottedEnemy)
        {
            spottedEnemy = true;
            StartCoroutine(CalmDown());
        }
    }

    private IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(calmDownTime);
        spottedEnemy = false;
    }

    public bool GetSpottedEnemy()
    {
        return spottedEnemy;
    }
}

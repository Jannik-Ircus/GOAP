using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorStorage : MonoBehaviour
{
    public string resource;
    public int currentStorage = 0;
    public int maxStorage = 10;
    [SerializeField]
    private GameObject[] storedWood;

    private void Start()
    {
        UpdateVisibleStorage();
    }

    public void ModifyStorage(int value)
    {
        currentStorage += value;
        UpdateVisibleStorage();
    }

    private void UpdateVisibleStorage()
    {
        for (int i = 0; i < maxStorage; i++)
        {
            if (i < currentStorage) storedWood[i].SetActive(true);
            else storedWood[i].SetActive(false);
        }
    }
}

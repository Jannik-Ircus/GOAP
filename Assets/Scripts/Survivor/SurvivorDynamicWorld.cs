using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicWorldItem
{
    public GameObject worldObject;
    [Range(0.1f, 10)]
    public float secondsBetweenSpawns = 3;
}

public class SurvivorDynamicWorld : MonoBehaviour
{
    public List<DynamicWorldItem> worldItems = new List<DynamicWorldItem>();

    public Vector3 spawnAreaCenter;
    public Vector2 spawnAreaSize;
    public LayerMask groundLayer;
    public int maxObjectsInScene = 25;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        foreach(DynamicWorldItem item in worldItems)
        {
            StartCoroutine(SpawnItem(item.worldObject, item.secondsBetweenSpawns));
        }
    }

    private IEnumerator SpawnItem(GameObject objectToSpawn, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(GetNumberOfSpawnedObjects() < maxObjectsInScene)
        {
            Vector3 spawnPosition = GetRandomPoint();
            if (spawnPosition != Vector3.zero)
            {
                GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(newObject);
            }
        }
        

        StartCoroutine(SpawnItem(objectToSpawn, delay));
    }

    private Vector3 GetRandomPoint()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        Vector3 randomPoint = new Vector3(spawnAreaCenter.x + randomX, spawnAreaCenter.y + 10, spawnAreaCenter.z + randomZ);

        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Return the point on the ground
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private int GetNumberOfSpawnedObjects()
    {
        int number = 0;
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach(GameObject gameObject in spawnedObjects)
        {
            if(gameObject==null)
            {
                objectsToRemove.Add(gameObject);
                continue;
            }
            number++;
        }

        foreach(GameObject gameObject in objectsToRemove)
        {
            spawnedObjects.Remove(gameObject);
        }
        return number;
    }

    /*void OnDrawGizmos()
    {
        // Set Gizmo color
        Gizmos.color = Color.red;

        // Calculate corners of the rectangle
        Vector3 topLeft = spawnAreaCenter + new Vector3(-spawnAreaSize.x / 2, 0, -spawnAreaSize.y / 2);
        Vector3 topRight = spawnAreaCenter + new Vector3(spawnAreaSize.x / 2, 0, -spawnAreaSize.y / 2);
        Vector3 bottomRight = spawnAreaCenter + new Vector3(spawnAreaSize.x / 2, 0, spawnAreaSize.y / 2);
        Vector3 bottomLeft = spawnAreaCenter + new Vector3(-spawnAreaSize.x / 2, 0, spawnAreaSize.y / 2);

        // Draw the edges of the rectangle
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }*/
}

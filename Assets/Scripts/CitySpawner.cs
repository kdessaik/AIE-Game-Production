using System.Collections.Generic;
using UnityEngine;

public class CitySpawner : MonoBehaviour
{
    [Header("References")]
    public Transform roadTransform;         // Assign your road here
    public Transform playerTransform;       // Assign player here
    public GameObject[] buildingPrefabs;    // Building prefabs

    [Header("Spawn Settings")]
    public float buildingOffset = 10f;      // Distance from road edge
    public int initialCount = 15;           // How many buildings to spawn initially
    public float minZSpacing = 20f;         // Min distance between buildings along Z
    public float maxZSpacing = 40f;         // Max distance between buildings
    public float spawnAheadDistance = 200f; // How far ahead of the player buildings spawn

    [Header("Movement")]
    public float moveSpeed = 150f;          // Speed toward player
    public float despawnBehindDistance = 50f; // How far behind the player to destroy

    [Header("Game Control")]
    public bool gameRunning = true;

    private List<GameObject> activeBuildings = new List<GameObject>();
    private float lastSpawnZ = 0f;

    void Start()
    {
        if (roadTransform == null || playerTransform == null)
        {
            Debug.LogError("Assign roadTransform and playerTransform!");
            return;
        }

        // Spawn initial buildings
        float spawnZ = playerTransform.position.z + spawnAheadDistance;
        for (int i = 0; i < initialCount; i++)
        {
            SpawnBuilding(spawnZ);
            spawnZ += Random.Range(minZSpacing, maxZSpacing);
        }
    }

    void Update()
    {
        if (!gameRunning) return;

        // Move buildings toward player
        for (int i = activeBuildings.Count - 1; i >= 0; i--)
        {
            GameObject building = activeBuildings[i];
            building.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            // Destroy if far behind player
            if (building.transform.position.z < playerTransform.position.z - despawnBehindDistance)
            {
                Destroy(building);
                activeBuildings.RemoveAt(i);

                // Spawn new building ahead
                float spawnZ = lastSpawnZ + Random.Range(minZSpacing, maxZSpacing);
                SpawnBuilding(spawnZ);
            }
        }
    }

    void SpawnBuilding(float zPos)
    {
        if (buildingPrefabs.Length == 0) return;

        GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

        // Road half-width
        float roadHalfWidth = roadTransform.localScale.x / 2f;

        // Random side
        float xLocal = Random.value > 0.5f
            ? (roadHalfWidth + buildingOffset)
            : -(roadHalfWidth + buildingOffset);

        Vector3 localPos = new Vector3(xLocal, 0, zPos);

        // Convert to world position using road transform
        Vector3 spawnPos = roadTransform.TransformPoint(localPos);

        GameObject newBuilding = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Random scale and Y rotation
        float scale = Random.Range(0.8f, 1.2f);
        newBuilding.transform.localScale = new Vector3(scale, scale, scale);
        newBuilding.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        activeBuildings.Add(newBuilding);
        lastSpawnZ = zPos;
    }

    // Stop buildings when game ends
    public void StopCity()
    {
        gameRunning = false;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class BuildingSpawner : MonoBehaviour
{
    public static BuildingSpawner Instance;

    [Header("References")]
    public Transform playerTransform;
    public Transform roadTransform;
    public GameObject[] buildingPrefabs;

    [Header("Spawn Settings")]
    public float buildingOffset = 10f; // distance from road edges
    public int initialCount = 10;
    public float spawnAhead = 150f;
    public float minSpacing = 25f;
    public float maxSpacing = 50f;

    [Header("Mover Settings")]
    public float buildingSpeed = 12f;
    public float despawnDistance = 80f;

    private float nextSpawnZ;
    private List<GameObject> activeBuildings = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        nextSpawnZ = playerTransform.position.z + spawnAhead;

        for (int i = 0; i < initialCount; i++)
        {
            SpawnBuilding();
        }
    }

    void SpawnBuilding()
    {
        if (buildingPrefabs.Length == 0) return;

        GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

        float roadHalfWidth = roadTransform.localScale.x / 2f;
        float x = (Random.value > 0.5f ? 1 : -1) * (roadHalfWidth + buildingOffset);

        Vector3 spawnPos = new Vector3(x, 0f, nextSpawnZ);

        // Convert local road position to world position
        spawnPos = roadTransform.TransformPoint(roadTransform.InverseTransformPoint(spawnPos));

        GameObject building = Instantiate(prefab, spawnPos, Quaternion.identity);
        activeBuildings.Add(building);

        // Add BuildingMover dynamically
        BuildingMover mover = building.GetComponent<BuildingMover>();
        if (mover == null) mover = building.AddComponent<BuildingMover>();

        mover.playerTransform = playerTransform;
        mover.roadTransform = roadTransform;
        mover.spawner = this;
        mover.speed = buildingSpeed;
        mover.despawnDistance = despawnDistance;

        nextSpawnZ += Random.Range(minSpacing, maxSpacing);
    }

    public void RespawnBuilding(GameObject building)
    {
        activeBuildings.Remove(building);
        Destroy(building);
        SpawnBuilding();
    }
}

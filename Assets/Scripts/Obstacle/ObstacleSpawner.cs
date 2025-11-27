using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform[] lanes; // Assign Lane1, Lane2, Lane3
    public GameObject[] obstaclePrefabs; // Cube, Sphere, Cylinder

    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2.5f;

    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnObstacle()
    {
        // Choose random lane
        Transform chosenLane = lanes[Random.Range(0, lanes.Length)];

        // Choose random obstacle (cube, sphere, cylinder)
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Spawn
        Instantiate(prefab, chosenLane.position, Quaternion.identity);
    }
}

using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Pools (set in inspector)")]
    public int[] poolOrder; // indices for PoolManager pools; (optional use)
    public Transform[] spawnPoints;    // assign lane spawn transforms
    public float baseSpawnInterval = 1.8f;
    public float randomSpawnVariance = 1.0f;
    public float difficultyRampEverySeconds = 10f;
    public float minSpawnInterval = 0.6f;
    public float spawnSpeedMin = 8f;
    public float spawnSpeedMax = 14f;

    float nextSpawnTime;
    float currentInterval;
    float lastRampTime;

    void Start()
    {
        currentInterval = baseSpawnInterval;
        nextSpawnTime = Time.time + currentInterval;
        lastRampTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomObstacle();
            nextSpawnTime = Time.time + currentInterval + Random.Range(-randomSpawnVariance, randomSpawnVariance);
            nextSpawnTime = Mathf.Max(nextSpawnTime, Time.time + minSpawnInterval);
        }

        // difficulty ramp
        if (Time.time - lastRampTime >= difficultyRampEverySeconds)
        {
            currentInterval = Mathf.Max(minSpawnInterval, currentInterval - 0.1f);
            lastRampTime = Time.time;
        }
    }

    void SpawnRandomObstacle()
    {
        if (PoolManager.Instance == null || spawnPoints.Length == 0 || PoolManager.Instance.pools.Length == 0) return;

        // choose random pool index
        int poolIndex = Random.Range(0, PoolManager.Instance.pools.Length);
        GameObject go = PoolManager.Instance.GetFromPool(poolIndex);
        if (go == null) return;

        // choose random lane / spawn point
        int laneIndex = Random.Range(0, spawnPoints.Length);
        Transform sp = spawnPoints[laneIndex];

        // set spawn pos: base spawn point x plus lane offset if spawn points centered
        Vector3 spawnPos = sp.position;
        // ensure prefab position aligns with lane X (spawnPoints are lane positions already)
        // initialize AI
        var ai = go.GetComponent<ObstacleAI>();
        float speed = Random.Range(spawnSpeedMin, spawnSpeedMax);
        if (ai != null)
        {
            // determine lane number from spawn point's x (assuming lanes at -laneWidth,0,+laneWidth)
            int laneNumber = Mathf.RoundToInt(spawnPos.x / ai.laneWidth);
            ai.Initialize(laneNumber, spawnPos, speed);
        }
        else
        {
            go.transform.position = spawnPos;
        }
    }
}

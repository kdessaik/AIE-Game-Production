// Esther Namulen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Lane1, Lane2, Lane3
    [Tooltip("Pool indices to pick from")]
    public int[] poolOrder;

    [Header("Spawn timing")]
    public float spawnIntervalMin = 1.0f;
    public float spawnIntervalMax = 2.0f;
    public float minSpawnLimit = 0.4f;
    public float intervalDecreaseRate = 0.02f;

    [Header("Base Speed (used once at spawn)")]
    public float spawnSpeedMin = 8f;
    public float spawnSpeedMax = 14f;

    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty difficulty = Difficulty.Normal;

    [System.Serializable]
    public struct DiffPreset
    {
        public float minInterval, maxInterval;
        public float minSpeed, maxSpeed;
    }

    public DiffPreset easy = new DiffPreset { minInterval = 1.6f, maxInterval = 2.6f, minSpeed = 8f, maxSpeed = 12f };
    public DiffPreset normal = new DiffPreset { minInterval = 1.0f, maxInterval = 1.8f, minSpeed = 10f, maxSpeed = 16f };
    public DiffPreset hard = new DiffPreset { minInterval = 0.6f, maxInterval = 1.2f, minSpeed = 14f, maxSpeed = 22f };

    void Start()
    {
        ApplyDifficultyPreset();
        StartCoroutine(SpawnLoop());
    }

    void ApplyDifficultyPreset()
    {
        DiffPreset p = difficulty switch
        {
            Difficulty.Easy => easy,
            Difficulty.Hard => hard,
            _ => normal
        };

        spawnIntervalMin = p.minInterval;
        spawnIntervalMax = p.maxInterval;
        spawnSpeedMin = p.minSpeed;
        spawnSpeedMax = p.maxSpeed;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Prevent spawning before game starts
            if (GameManager.Instance != null && !GameManager.Instance.isGameStarted)
            {
                yield return null;
                continue;
            }

            SpawnWave();

            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);

            // Gradually increase difficulty over time
            spawnIntervalMin = Mathf.Max(minSpawnLimit,
                spawnIntervalMin - intervalDecreaseRate * Time.deltaTime);
            spawnIntervalMax = Mathf.Max(minSpawnLimit + 0.1f,
                spawnIntervalMax - intervalDecreaseRate * Time.deltaTime);
        }
    }

    void SpawnWave()
    {
        if (spawnPoints == null || spawnPoints.Length < 3) return;
        if (PoolManager.Instance == null) return;

        // Pick one free lane
        int freeLane = Random.Range(0, spawnPoints.Length);

        List<int> lanesToSpawn = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
            if (i != freeLane) lanesToSpawn.Add(i);

        foreach (int laneIndex in lanesToSpawn)
        {
            Transform sp = spawnPoints[laneIndex];
            if (sp == null) continue;

            int poolIndex = ChoosePoolIndexRandom();
            GameObject go = PoolManager.Instance.GetFromPool(poolIndex);
            if (go == null) continue;

            go.transform.position = sp.position;
            go.transform.rotation = sp.rotation;

            float baseSpeed = Random.Range(spawnSpeedMin, spawnSpeedMax);

            // ObstacleMover controls movement
            var mover = go.GetComponent<ObstacleMover>();
            if (mover == null)
                mover = go.AddComponent<ObstacleMover>();

            mover.baseSpeed = baseSpeed;
        }
    }

    int ChoosePoolIndexRandom()
    {
        if (poolOrder != null && poolOrder.Length > 0)
            return poolOrder[Random.Range(0, poolOrder.Length)];

        return Random.Range(0, PoolManager.Instance.pools.Length);
    }

    public void SetDifficulty(Difficulty d)
    {
        difficulty = d;
        ApplyDifficultyPreset();
    }
}

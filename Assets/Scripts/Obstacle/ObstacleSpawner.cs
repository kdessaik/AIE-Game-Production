//Esther Namulen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Lane1, Lane2, Lane3 (size must be 3)
    [Tooltip("Pool indices to pick from; set in inspector (PoolManager pools order)")]
    public int[] poolOrder; // optional order mapping of pools to use; if empty uses all pools

    [Header("Spawn timing")]
    public float spawnIntervalMin = 1.0f;
    public float spawnIntervalMax = 2.0f;
    public float minSpawnLimit = 0.4f;
    public float intervalDecreaseRate = 0.02f;

    [Header("Speed")]
    public float spawnSpeedMin = 8f;
    public float spawnSpeedMax = 14f;
    public float speedIncreaseRate = 0.1f;

    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty difficulty = Difficulty.Normal;

    // difficulty presets (you can tweak)
    [System.Serializable] public struct DiffPreset { public float minInterval, maxInterval, minSpeed, maxSpeed; }
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
        switch (difficulty)
        {
            case Difficulty.Easy:
                spawnIntervalMin = easy.minInterval;
                spawnIntervalMax = easy.maxInterval;
                spawnSpeedMin = easy.minSpeed;
                spawnSpeedMax = easy.maxSpeed;
                break;
            case Difficulty.Normal:
                spawnIntervalMin = normal.minInterval;
                spawnIntervalMax = normal.maxInterval;
                spawnSpeedMin = normal.minSpeed;
                spawnSpeedMax = normal.maxSpeed;
                break;
            case Difficulty.Hard:
                spawnIntervalMin = hard.minInterval;
                spawnIntervalMax = hard.maxInterval;
                spawnSpeedMin = hard.minSpeed;
                spawnSpeedMax = hard.maxSpeed;
                break;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave(); // spawn two obstacles (leave one lane free)

            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);

            // dynamic difficulty scaling over time
            spawnSpeedMin += speedIncreaseRate * Time.deltaTime;
            spawnSpeedMax += speedIncreaseRate * Time.deltaTime;

            spawnIntervalMin = Mathf.Max(minSpawnLimit, spawnIntervalMin - intervalDecreaseRate * Time.deltaTime);
            spawnIntervalMax = Mathf.Max(minSpawnLimit + 0.1f, spawnIntervalMax - intervalDecreaseRate * Time.deltaTime);
        }
    }

    void SpawnWave()
    {
        if (spawnPoints == null || spawnPoints.Length < 3) return;
        if (PoolManager.Instance == null || PoolManager.Instance.pools == null) return;

        // choose one lane to be free for player
        int freeLane = Random.Range(0, spawnPoints.Length);

        // prepare list of lane indices to spawn
        List<int> lanesToSpawn = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++) if (i != freeLane) lanesToSpawn.Add(i);

        // shuffle lanesToSpawn for randomness
        for (int i = 0; i < lanesToSpawn.Count; i++)
        {
            int r = Random.Range(i, lanesToSpawn.Count);
            int tmp = lanesToSpawn[i];
            lanesToSpawn[i] = lanesToSpawn[r];
            lanesToSpawn[r] = tmp;
        }

        // spawn in each lane (two lanes)
        for (int j = 0; j < lanesToSpawn.Count; j++)
        {
            int laneIndex = lanesToSpawn[j];
            Transform sp = spawnPoints[laneIndex];
            if (sp == null) continue;

            // choose pool index
            int poolIndex = ChoosePoolIndexRandom();
            GameObject go = PoolManager.Instance.GetFromPool(poolIndex);
            if (go == null) continue;

            Vector3 spawnPos = sp.position;
            go.transform.position = spawnPos;
            go.transform.rotation = sp.rotation;

            // if has ObstacleAI, initialize it; laneNumber mapping: center=0, left=-1, right=+1
            var ai = go.GetComponent<ObstacleAI>();
            float speed = Random.Range(spawnSpeedMin, spawnSpeedMax);
            if (ai != null)
            {
                int laneNumber = laneIndex - 1; // if lanes are 0,1,2 -> map to -1,0,1
                ai.Initialize(laneNumber, spawnPos, speed);
            }
            else
            {
                // fallback mover
                var mover = go.GetComponent<ObstacleMover>();
                if (mover == null) mover = go.AddComponent<ObstacleMover>();
                mover.speed = speed;
            }
        }
    }

    int ChoosePoolIndexRandom()
    {
        if (poolOrder != null && poolOrder.Length > 0)
        {
            return poolOrder[Random.Range(0, poolOrder.Length)];
        }
        else
        {
            return Random.Range(0, PoolManager.Instance.pools.Length);
        }
    }

    // optional: allow switching difficulty at runtime
    public void SetDifficulty(Difficulty d)
    {
        difficulty = d;
        ApplyDifficultyPreset();
    }
}

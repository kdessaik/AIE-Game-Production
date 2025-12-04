using UnityEngine;

public class ObstacleAI : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 12f;         // how fast it moves toward -Z
    public float laneChangeSpeed = 6f;       // lateral speed when changing lane
    public float laneWidth = 3f;             // x distance between lanes

    [Header("Lane logic")]
    public int currentLane = 0;              // -1 left, 0 center, 1 right
    public int maxLanes = 1;                 // lanes each side (1 => -1,0,1)
    public float minDecisionTime = 1f;
    public float maxDecisionTime = 3f;
    float nextDecisionTime;
    Vector3 laneTarget;
    bool changingLane = false;

    [Header("Lifecycle")]
    public float destroyZ = -30f;            // when past this Z, return to pool or destroy
    bool scored = false;                    // whether this obstacle was counted for score

    void OnEnable()
    {
        // reset state
        scored = false;
        nextDecisionTime = Time.time + Random.Range(minDecisionTime, maxDecisionTime);
        laneTarget = transform.position;
        changingLane = false;
    }

    void Update()
    {
        // forward move (toward negative Z)
        transform.Translate(Vector3.back * forwardSpeed * Time.deltaTime, Space.World);

        // lateral movement if changing lanes
        if (changingLane)
        {
            transform.position = Vector3.MoveTowards(transform.position, laneTarget, laneChangeSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, laneTarget) < 0.05f)
            {
                changingLane = false;
                nextDecisionTime = Time.time + Random.Range(minDecisionTime, maxDecisionTime);
            }
        }
        else
        {
            if (Time.time >= nextDecisionTime)
                DecideLaneChange();
        }

        // lifecycle: if behind destroyZ -> return to pool
        if (transform.position.z <= destroyZ)
        {
            // if it wasn't scored (missed), we don't do anything here; scoring handled by ScoreLine trigger
            PoolManager.Instance?.ReturnToPool(gameObject);
        }
    }

    void DecideLaneChange()
    {
        // 50% chance to attempt a lane change
        if (Random.value < 0.5f)
        {
            int dir = Random.value < 0.5f ? -1 : 1;
            int newLane = Mathf.Clamp(currentLane + dir, -maxLanes, maxLanes);
            if (newLane != currentLane)
            {
                currentLane = newLane;
                Vector3 pos = transform.position;
                pos.x = currentLane * laneWidth;
                laneTarget = new Vector3(pos.x, pos.y, pos.z);
                changingLane = true;
            }
        }
        nextDecisionTime = Time.time + Random.Range(minDecisionTime, maxDecisionTime);
    }

    // Called by ScoreLine when it passes through (optional)
    public void MarkScored()
    {
        if (scored) return;
        scored = true;
        // optionally play effect before return to pool
    }

    // Called externally to reset lane and position when reused
    public void Initialize(int lane, Vector3 spawnPos, float speed)
    {
        currentLane = lane;
        forwardSpeed = speed;
        laneTarget = spawnPos;
        transform.position = spawnPos;
        changingLane = false;
        nextDecisionTime = Time.time + Random.Range(minDecisionTime, maxDecisionTime);
        scored = false;
    }
}

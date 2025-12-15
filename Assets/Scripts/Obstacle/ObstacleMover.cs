// Esther Namulen
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [Header("Speed")]
    public float baseSpeed = 12f;          // speed set by spawner
    public float speedStep = 1.5f;         // added every 5 score
    public int scoreStep = 5;

    public float destroyZ = -20f;

    private bool hasScored = false;

    void OnEnable()
    {
        hasScored = false;
    }

    void Update()
    {
        // Stop movement if game not started or paused
        if (GameManager.Instance != null && !GameManager.Instance.isGameStarted) return;
        if (Time.timeScale == 0f) return;

        float currentSpeed = GetCurrentSpeed();

        transform.Translate(Vector3.back * currentSpeed * Time.deltaTime, Space.World);

        // Score when enemy safely passes player
        if (!hasScored && transform.position.z < 0f)
        {
            hasScored = true;
            ScoreManager.Instance?.AddScore(1);
        }

        // Return to pool
        if (transform.position.z <= destroyZ)
        {
            gameObject.SetActive(false);
        }
    }

    float GetCurrentSpeed()
    {
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.score : 0;
        int steps = score / scoreStep; // every 5 points
        return baseSpeed + (steps * speedStep);
    }
}

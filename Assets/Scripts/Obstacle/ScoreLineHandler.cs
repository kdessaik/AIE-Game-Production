using UnityEngine;

public class ScoreLineHandler : MonoBehaviour
{
    public GameManager gameManager;

    void Reset()
    {
        // try to auto-find GameManager if exists in scene
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // mark scored on obstacle if it has AI
            var ai = other.GetComponent<ObstacleAI>();
            if (ai != null) ai.MarkScored();

            gameManager?.AddScore(1);

            // return obstacle to pool
            PoolManager.Instance?.ReturnToPool(other.gameObject);
        }
    }
}

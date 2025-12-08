//Esther Namulen
using UnityEngine;

public class ScoreLineHandler : MonoBehaviour
{
    public GameManager gameManager;

    void Reset()
    {
#if UNITY_2021_2_OR_NEWER
        if (gameManager == null) gameManager = GameObject.FindFirstObjectByType<GameManager>();
#else
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
#endif
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;

        var ai = other.GetComponent<ObstacleAI>();
        if (ai != null) ai.MarkScored();

        gameManager?.AddScore(1);

        PoolManager.Instance?.ReturnToPool(other.gameObject);
    }
}

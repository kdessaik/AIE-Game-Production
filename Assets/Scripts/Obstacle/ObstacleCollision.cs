// Esther Namulen
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    [Header("Player Settings")]
    public int requiredScore = 10;   // Minimum score before collisions count
    public int damage = 1;           // Life lost per hit

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Check if score requirement is met
        if (GameManager.Instance != null && GameManager.Instance.score < requiredScore)
            return;

        // Reduce player health
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Remove obstacle (disable or return to pool)
        gameObject.SetActive(false);
        // If using pooling instead, replace above line with:
        // PoolManager.Instance?.ReturnToPool(gameObject);
    }
}

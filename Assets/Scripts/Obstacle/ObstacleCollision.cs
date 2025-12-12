// Esther Namulen
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    [Header("Player Settings")]
    public int requiredScore = 10;       // Minimum score before collisions count
    public int damage = 1;               // Life lost per hit

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats == null) return;

            if (playerStats.Score >= requiredScore)
            {
                playerStats.Life -= damage;
                Debug.Log("Player hit! Life: " + playerStats.Life);

                // Instead of Destroy(gameObject), return to pool
                if (PoolManager.Instance != null)
                    PoolManager.Instance.ReturnToPool(gameObject);
                else
                    Destroy(gameObject); // fallback if no pool

                if (playerStats.Life <= 0)
                {
                    Debug.Log("Player died! Game Over!");
                }
            }
        }
    }

}
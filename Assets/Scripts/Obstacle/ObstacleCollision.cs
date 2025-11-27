using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // If you have a GameOver manager, call it here.
    // If not, replace with Debug.Log for now.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If you have a GameOver manager, use:
            // GameOverManager.Instance?.GameOver();

            Debug.Log("Player collided with obstacle - game over (placeholder).");
        }
    }
}

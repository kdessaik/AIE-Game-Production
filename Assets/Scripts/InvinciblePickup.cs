using UnityEngine;

public class InvinciblePickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float invincibilityDuration = 3f;  // seconds

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player touched the pickup
        PlayerLaneSwitching player = other.GetComponent<PlayerLaneSwitching>();
        if (player != null)
        {
            // Make player invincible
            player.MakeInvincibleForSeconds(invincibilityDuration);

            // Optionally destroy the pickup
            Destroy(gameObject);
        }
    }
}

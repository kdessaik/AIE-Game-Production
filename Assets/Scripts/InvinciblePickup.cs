// Made by Samuel Lawrence

using UnityEngine;

public class InvinciblePickup : MonoBehaviour
{
    public int speed = 10;
    public Vector3 direction;
    public float invincibilityDuration = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        
        if (obj != null)
        {
            if (obj.tag == "Player")
            {
                PlayerLaneSwitching playerScript = obj.GetComponent<PlayerLaneSwitching>();

                if (playerScript != null)
                {
                    playerScript.MakeInvincibleForSeconds(invincibilityDuration);
                    Destroy(gameObject);
                }
            }
        }
    }
}

// Esther Namulen
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 12f;
    public float destroyZ = -20f; // behind player threshold
    private const float fixedY = 0.3f; // forced Y position for all obstacles

    void Update()
    {
        // ALWAYS force Y = 0.3f (for perfect alignment with the road)
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;

        // Move obstacle backward
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        // Destroy / return to pool when behind player
        if (transform.position.z <= destroyZ)
        {
            PoolManager.Instance?.ReturnToPool(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger game over
            GameOverManager.Instance?.GameOver();

            // Return obstacle to pool
            PoolManager.Instance?.ReturnToPool(gameObject);
        }
    }
}

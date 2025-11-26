using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float destroyZ = -20f; // When obstacle goes behind player

    void Update()
    {
        // Move opposite direction of player (player moves forward, obstacle moves backward)
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        // Destroy when behind player
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
            ScoreManager.Instance.AddScore(1); // Player earns score for passing
        }
    }
}

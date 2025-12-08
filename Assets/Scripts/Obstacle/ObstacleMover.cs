//Esther Namulen
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 12f;
    public float destroyZ = -20f; // behind player threshold

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z <= destroyZ)
        {
            // prefer returning to pool if available
            PoolManager.Instance?.ReturnToPool(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // call your game over manager if you have one
            GameOverManager.Instance?.GameOver();

            // return to pool
            PoolManager.Instance?.ReturnToPool(gameObject);
        }
    }
}

using UnityEngine;

public class BuildingMover : MonoBehaviour
{
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Transform roadTransform;
    [HideInInspector] public float speed;
    [HideInInspector] public float despawnDistance;
    [HideInInspector] public BuildingSpawner spawner;

    void Update()
    {
        if (playerTransform == null || roadTransform == null) return;

        // Move along road forward
        transform.Translate(roadTransform.forward * speed * Time.deltaTime, Space.World);

        // If behind player ? respawn
        if (Vector3.Distance(transform.position, playerTransform.position) > despawnDistance)
        {
            spawner?.RespawnBuilding(gameObject);
        }
    }
}

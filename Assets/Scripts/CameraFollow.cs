using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 followOffset = new Vector3(0, 4, -10);  // your camera offset
    public float smoothSpeed = 5f;

    void Start()
    {
        // Set initial camera position and rotation
        transform.position = new Vector3(0, 4, -10);
        transform.rotation = Quaternion.Euler(14f, 0f, 0f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Smooth follow
        Vector3 desiredPosition = target.position + followOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Keep camera rotation stable
        transform.rotation = Quaternion.Euler(14f, 0f, 0f);
    }
}

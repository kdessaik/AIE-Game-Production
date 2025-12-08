// Kambale Kibeho Dessai
using UnityEngine;

public class RoadMoverLoop : MonoBehaviour
{
    public float playerSpeed = 10f;       // speed at which road moves
    public float resetY = -25000f;        // Y position behind the player
    public float segmentHeight = 25000f;  // height of one road segment
    private float timer = 6f;             // countdown for reset

    // Target reset position
    private Vector3 initialPosition = new Vector3(0.57f, 0.1f, 235f);

    void Start()
    {
        // Optional: move road to initial position at start
        transform.position = initialPosition;
    }

    void Update()
    {
        // Move the road backward along its local "up" (because of rotation)
        transform.Translate(transform.up * playerSpeed * Time.deltaTime, Space.World);

        // Loop the road if it goes behind resetY
        if (transform.position.y <= resetY)
        {
            Vector3 pos = transform.position;
            pos.y += segmentHeight * 2; // place above the other segment
            transform.position = pos;
        }

        // Countdown timer
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Reset road to initial position
            transform.position = initialPosition;

            // Reset timer
            timer = 5f;
        }
    }
}

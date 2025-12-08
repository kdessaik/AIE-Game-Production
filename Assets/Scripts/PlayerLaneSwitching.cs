using System.Collections;
using UnityEngine;

public class PlayerLaneSwitching : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    public GameObject bumper; // visual for invincibility

    [Header("Lanes")]
    public Vector3[] LanePositions = new Vector3[3] { new Vector3(-3, 0, 0), new Vector3(0, 0, 0), new Vector3(3, 0, 0) };
    public int CurrentLane = 1;
    public float LaneMoveSpeed = 10f;
    public float SnapThreshold = 0.05f;

    [Header("Movement")]
    public float ForwardSpeed = 5f;
    public float JumpHeight = 2f;       // height of hop
    public float JumpSpeed = 5f;        // speed of hop
    public float GroundY = 0.9f;

    [Header("Invincibility")]
    public bool Invincible { get; private set; } = false;

    private Vector3 velocity = Vector3.zero;
    private bool isGrounded = true;

    void Start()
    {
        if (Player == null) Player = transform;

        // Start at middle lane
        Vector3 startPos = LanePositions[CurrentLane];
        startPos.y = GroundY;
        Player.position = startPos;

        UpdateBumper();
    }

    void Update()
    {
        HandleLaneInput();
        HandleForwardBackInput();
        HandleJumpInput();
        SmoothLaneMovement();
    }

    // ---------------- Lane Switching ----------------
    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);
    }

    void ChangeLane(int direction)
    {
        CurrentLane = Mathf.Clamp(CurrentLane + direction, 0, LanePositions.Length - 1);
    }

    void SmoothLaneMovement()
    {
        Vector3 targetPos = new Vector3(LanePositions[CurrentLane].x, Player.position.y, Player.position.z);
        Player.position = Vector3.SmoothDamp(Player.position, targetPos, ref velocity, 1f / LaneMoveSpeed);

        if (Mathf.Abs(Player.position.x - targetPos.x) <= SnapThreshold)
        {
            Player.position = new Vector3(targetPos.x, Player.position.y, Player.position.z);
            velocity.x = 0f;
        }
    }

    // ---------------- Forward/Back ----------------
    void HandleForwardBackInput()
    {
        float moveZ = 0f;
        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;

        Player.Translate(Vector3.forward * moveZ * ForwardSpeed * Time.deltaTime, Space.World);
    }

    // ---------------- Jump (Hop) ----------------
    void HandleJumpInput()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            StopAllCoroutines();
            StartCoroutine(HopRoutine());
        }
    }

    private IEnumerator HopRoutine()
    {
        isGrounded = false;

        Vector3 startPos = Player.position;
        Vector3 targetPos = startPos + new Vector3(0, JumpHeight, 0);

        // Move up
        while (Vector3.Distance(Player.position, targetPos) > 0.01f)
        {
            Player.position = Vector3.MoveTowards(Player.position, targetPos, JumpSpeed * Time.deltaTime);
            yield return null;
        }

        // Move back down
        targetPos = new Vector3(Player.position.x, GroundY, Player.position.z);
        while (Vector3.Distance(Player.position, targetPos) > 0.01f)
        {
            Player.position = Vector3.MoveTowards(Player.position, targetPos, JumpSpeed * Time.deltaTime);
            yield return null;
        }

        Player.position = new Vector3(Player.position.x, GroundY, Player.position.z);
        isGrounded = true;
    }

    // ---------------- Invincibility ----------------
    public void MakeInvincibleForSeconds(float seconds)
    {
        EnableInvincibility();
        StartCoroutine(DisableInvincibilityAfter(seconds));
    }

    private IEnumerator DisableInvincibilityAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DisableInvincibility();
    }

    void EnableInvincibility()
    {
        Invincible = true;
        UpdateBumper();
    }

    void DisableInvincibility()
    {
        Invincible = false;
        UpdateBumper();
    }

    void UpdateBumper()
    {
        if (bumper != null) bumper.SetActive(Invincible);
    }

    // ---------------- Collision / Death ----------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TryKill();
        }
    }

    void TryKill()
    {
        if (!Invincible)
        {
            Destroy(gameObject); // player dies
        }
    }
}

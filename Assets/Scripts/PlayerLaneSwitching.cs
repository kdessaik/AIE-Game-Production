using System;
using System.Collections;
using UnityEngine;

public class PlayerLaneSwitching : MonoBehaviour
{
    [Header("References")]
    public Transform Player;                 // visible mesh/graphic to move
    public GameObject bumper;                // invincibility visual

    [Header("Lanes")]
    public Vector3[] LanePositions = new Vector3[3]
    {
        new Vector3(-3f, 0f, 0f),
        new Vector3( 0f, 0f, 0f),
        new Vector3( 3f, 0f, 0f)
    };
    public int CurrentLane = 1;              // start in middle by default

    [Header("Movement")]
    public float moveSpeed = 10f;            // higher = faster smoothing
    public float snapThreshold = 0.05f;      // consider reached when closer than this

    [Header("Invincibility")]
    public bool Invincible { get; private set; } = false;
    public float defaultInvincibleSeconds = 3f;

    // Private smoothing state
    Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Ensure Player reference — if not provided, assume this transform
        if (Player == null) Player = transform;

        // Ensure starting lane position
        if (CurrentLane < 0 || CurrentLane >= LanePositions.Length) CurrentLane = 1;

        // Set starting position and FORCE Y = 0.9f
        Vector3 startPos = LanePositions[CurrentLane];
        startPos.y = 0.9f;
        Player.position = startPos;

        UpdateBumperVisibility();
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleTouchSwipe();   // works on mobile
        SmoothMoveToLane();
    }

    // ---------------- Input ----------------
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLane(-1);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveLane(1);
    }

    // Simple touch swipe left/right (one-finger)
    Vector2 touchStart;
    bool touchActive = false;
    void HandleTouchSwipe()
    {
        if (Input.touchCount == 0)
        {
            touchActive = false;
            return;
        }

        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
        {
            touchStart = t.position;
            touchActive = true;
        }
        else if (touchActive && (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
        {
            Vector2 delta = t.position - touchStart;
            float absX = Mathf.Abs(delta.x);
            float absY = Mathf.Abs(delta.y);

            // require horizontal swipe bigger than vertical and threshold
            if (absX > absY && absX > 50f)
            {
                if (delta.x > 0) MoveLane(1);
                else MoveLane(-1);
            }

            touchActive = false;
        }
    }

    // ---------------- Lane movement ----------------
    public void SetLane(int newLane)
    {
        if (newLane >= 0 && newLane < LanePositions.Length)
        {
            CurrentLane = newLane;
        }
    }

    public void MoveLane(int direction)
    {
        int newLane = Mathf.Clamp(CurrentLane + direction, 0, LanePositions.Length - 1);
        SetLane(newLane);
    }

    void SmoothMoveToLane()
    {
        // Get target lane
        Vector3 target = LanePositions[CurrentLane];

        // Force the correct Y height
        target.y = 0.9f;

        // SmoothDamp for nice feel
        Player.position = Vector3.SmoothDamp(Player.position, target, ref velocity, 1f / moveSpeed);

        // Snap when close
        Vector3 correctedCurrent = Player.position;
        correctedCurrent.y = 0.3f; // force Y every frame
        Player.position = correctedCurrent;

        if (Vector3.Distance(Player.position, target) <= snapThreshold)
        {
            Player.position = target;
            velocity = Vector3.zero;
        }
    }

    // ---------------- Death & Invincibility ----------------
    private void ForceKill()
    {
        Destroy(gameObject);
    }

    public void TryKill()
    {
        if (!Invincible)
        {
            ForceKill();
        }
    }

    private IEnumerator Call(Action func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func?.Invoke();
    }

    private void CallAfterDelay(Action func, float delay)
    {
        StartCoroutine(Call(func, delay));
    }

    void UpdateBumperVisibility()
    {
        if (bumper != null) bumper.SetActive(Invincible);
    }

    void EnableInvincibility()
    {
        Invincible = true;
        UpdateBumperVisibility();
    }

    void DisableInvincibility()
    {
        Invincible = false;
        UpdateBumperVisibility();
    }

    public void MakeInvincibleForSeconds(float seconds)
    {
        EnableInvincibility();
        CallAfterDelay(() => DisableInvincibility(), seconds);
    }

    public void MakeInvincibleDefault()
    {
        MakeInvincibleForSeconds(defaultInvincibleSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            TryKill();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TryKill();
        }
    }
}

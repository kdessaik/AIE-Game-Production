// Made by Samuel Lawrence

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaneSwitching : MonoBehaviour
{
    [Header("References")]
    public Transform Player;                // Player object (car mesh)
    public GameObject bumper;               // Invincibility visual

    [Header("Lanes")]
    public Vector3[] LanePositions;         // X positions for lanes
    public int CurrentLane = 1;             // Start in middle lane

    [Header("Settings")]
    private float fixedY = 0.16f;           // Lock Y position

    [Header("Invincibility")]
    public bool Invincible { get; private set; } = false;
    public float defaultInvincibleSeconds = 3f;

    // Private smoothing variables
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Lock player to fixed Y at start
        if (Player == null) Player = transform;

        if (LanePositions.Length == 0)
            LanePositions = new Vector3[3] {
                new Vector3(-2f, fixedY, 0f),
                new Vector3(0f, fixedY, 0f),
                new Vector3(2f, fixedY, 0f)
            };

        // Start in middle lane
        SetLane(CurrentLane);
        UpdateBumperVisibility();
    }

    void Update()
    {
        HandleInput();

        // Always lock Y
        Vector3 pos = Player.position;
        pos.y = fixedY;
        Player.position = pos;
    }

    // ---------------- Input ----------------
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLane(-1);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveLane(1);
    }

    // ---------------- Lane movement ----------------
    public void SetLane(int newLane)
    {
        if (newLane >= 0 && newLane < LanePositions.Length)
        {
            Vector3 pos = LanePositions[newLane];
            pos.y = fixedY;  // lock Y
            Player.position = pos;
            CurrentLane = newLane;
        }
    }

    public void MoveLane(int direction)
    {
        int newLane = Mathf.Clamp(CurrentLane + direction, 0, LanePositions.Length - 1);
        SetLane(newLane);
    }

    // ---------------- Death & Invincibility ----------------
    private void ForceKill()
    {
        Destroy(Player);
    }

    public void TryKill()
    {
        if (!Invincible)
            ForceKill();
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

    private void UpdateBumperVisibility()
    {
        if (bumper != null)
            bumper.SetActive(Invincible);
    }

    private void EnableInvincibility()
    {
        Invincible = true;
        UpdateBumperVisibility();
    }

    private void DisableInvincibility()
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
}

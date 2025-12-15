// Made by Samuel Lawrence (Modified for smooth movement)

using System;
using System.Collections;
using UnityEngine;

public class PlayerLaneSwitching : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    public GameObject bumper;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;          // Horizontal speed
    public float roadLimitX =3.5f;       // Left & right road boundary
    private float fixedY = 0.16f;

    [Header("Invincibility")]
    public bool Invincible { get; private set; } = false;
    public float defaultInvincibleSeconds = 3f;

    [Header("Road Reference")]
    public Transform road;          // Assign road GameObject
    public float edgePadding = 0.5f; // Space from road edges

    void Start()
    {
        if (Player == null)
            Player = transform;

        CalculateRoadLimits();
        UpdateBumperVisibility();
    }

    void CalculateRoadLimits()
    {
        if (road == null)
        {
            Debug.LogError("Road is NOT assigned!");
            return;
        }

        // Real road width in world units
        float roadWidth = road.localScale.x;

        // Player width
        float playerWidth = Player.localScale.x;

        roadLimitX = (roadWidth / 2f) - (playerWidth / 2f) - edgePadding;
        
    }

    void Update()
    {
       

        HandleInput();

        Vector3 pos = Player.position;
        pos.y = fixedY;
        Player.position = pos;
    }

    // ---------------- Smooth Input ----------------
    private void HandleInput()
    {
        float horizontal =
            (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f :
            (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f : 0f;

        Player.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime, Space.World);

        Vector3 pos = Player.position;
        pos.x = Mathf.Clamp(pos.x, -roadLimitX, roadLimitX);
        Player.position = pos;
    }

    // ---------------- Death & Invincibility ----------------
    private void ForceKill()
    {
        Destroy(Player.gameObject);
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

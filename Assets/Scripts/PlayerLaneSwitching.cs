// Made by Samuel Lawrence

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaneSwitching : MonoBehaviour
{
    public Transform Player;
    public int CurrentLane = 0;
    public Vector3[] LanePositions;

    public bool Invincible { get; private set; } = false;
    public GameObject bumper;

    // sets the player's lane, 0 for left lane, 1 for middle lane, 2 for right lane
    public void SetLane(int NewLane)
    {
        if (NewLane >= 0 && NewLane < LanePositions.Length)
        {
            Player.position = LanePositions[NewLane];
            CurrentLane = NewLane;
        }
    }

    // moves the player's lane in a direction, -1 for left, 1 for right.
    public void MoveLane(int Direction)
    {
        int NewLane = CurrentLane + Direction;

        SetLane(NewLane);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveLane(1);
        }
    }

    private void ForceKill()
    {
        Destroy(Player);

        // TODO: display to the player that they have died on the UI (with help from others)
    }

    // kills the player if they are not invincible. obstacle scripts can call this function when the player collides with an obstacle.
    public void TryKill()
    {
        if (Invincible == false)
        {
            ForceKill();
        }
    }

    private IEnumerator Call(Action func, float delay)
    {
        yield return new WaitForSeconds(delay);

        func.Invoke();
    }

    private void CallAfterDelay(Action func, float delay)
    {
        StartCoroutine(Call(func, delay));
    }

    private void UpdateBumperVisibility()
    {
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
}

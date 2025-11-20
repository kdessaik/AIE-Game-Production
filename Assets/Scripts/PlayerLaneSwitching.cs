// Made by Samuel Lawrence

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaneSwitching : MonoBehaviour
{
    public Transform Player;
    public int CurrentLane = 0;
    public Vector3[] LanePositions;

    public void SetLane(int NewLane)
    {
        if (NewLane >= 0 && NewLane < LanePositions.Length)
        {
            Player.position = LanePositions[NewLane];
            CurrentLane = NewLane;
        }
    }

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
}

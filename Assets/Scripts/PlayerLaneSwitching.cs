// Made by Samuel Lawrence

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaneSwitching : MonoBehaviour
{
    public Vector3[] lanePositions;

    private Vector2 move;

    // This method gets called automatically by PlayerInput
    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        Debug.Log(move);
    }
}

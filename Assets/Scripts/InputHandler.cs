using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action CancelEvent;

    Controls controls;

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        DodgeEvent?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        MovementValue = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
    }

    public void OnTarget(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            IsAttacking = true;
        else if (ctx.canceled)
            IsAttacking = false;
    }

    public void OnBlock(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            IsBlocking = true;
        else if (ctx.canceled)
            IsBlocking = false;
    }
}

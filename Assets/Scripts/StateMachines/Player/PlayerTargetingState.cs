using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    readonly int TargetingForwardSpeedHash = Animator.StringToHash("TargetingForwardSpeed");
    readonly int TargetingRightSpeedHash = Animator.StringToHash("TargetingRightSpeed");

    const float AnimatorDampTime = .1f;
    const float CrossfadeDuration = .1f;

    Vector2 dodgeDirection;
    float dodgeDuration;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.InputHandler.CancelEvent += InputHandler_CancelEvent;
        stateMachine.InputHandler.DodgeEvent += InputHandler_DodgeEvent;
        stateMachine.InputHandler.JumpEvent += InputHandler_JumpEvent;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputHandler.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
            return;
        }

        if (stateMachine.InputHandler.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputHandler.CancelEvent -= InputHandler_CancelEvent;
        stateMachine.InputHandler.DodgeEvent -= InputHandler_DodgeEvent;
        stateMachine.InputHandler.JumpEvent -= InputHandler_JumpEvent;
    }

    void InputHandler_CancelEvent()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    void InputHandler_DodgeEvent()
    {
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown)
            return;

        stateMachine.SetDodgeTime(Time.time);
        dodgeDirection = stateMachine.InputHandler.MovementValue;
        dodgeDuration = stateMachine.DodgeDuration;

        // default to a back step
        if (dodgeDirection == Vector2.zero)
            dodgeDirection = Vector2.down;
    }

    void InputHandler_JumpEvent()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        if (dodgeDuration > 0f)
        {
            movement += stateMachine.transform.right * dodgeDirection.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
            movement += stateMachine.transform.forward * dodgeDirection.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

            dodgeDuration = Mathf.Max(dodgeDuration - deltaTime, 0f);
        }
        else
        {
            movement += stateMachine.transform.right * stateMachine.InputHandler.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputHandler.MovementValue.y;
        }

        return movement;
    }

    void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputHandler.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardSpeedHash, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(TargetingForwardSpeedHash, stateMachine.InputHandler.MovementValue.y > 0 ? 1f : -1f, AnimatorDampTime, deltaTime);
        }

        if (stateMachine.InputHandler.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightSpeedHash, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(TargetingRightSpeedHash, stateMachine.InputHandler.MovementValue.x > 0 ? 1f : -1f, AnimatorDampTime, deltaTime);
        }
    }
}

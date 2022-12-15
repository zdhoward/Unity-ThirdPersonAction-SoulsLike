using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = .1f;
    private const float CrossfadeDuration = .1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.InputHandler.TargetEvent += InputHandler_TargetEvent;

        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputHandler.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputHandler.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }

        FaceMovementDirection(movement, deltaTime);
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputHandler.TargetEvent -= InputHandler_TargetEvent;
    }

    void InputHandler_JumpEvent()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    void InputHandler_TargetEvent()
    {
        if (!stateMachine.Targeter.SelectTarget())
            return;

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    void FaceMovementDirection(Vector3 movementDirection, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movementDirection),
            stateMachine.RotationDamping * deltaTime);
    }

    Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputHandler.MovementValue.y + right * stateMachine.InputHandler.MovementValue.x;
    }
}

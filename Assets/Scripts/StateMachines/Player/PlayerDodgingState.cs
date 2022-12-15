using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgingBlendTreeHash = Animator.StringToHash("DodgingBlendTree");
    private readonly int DodgingForwardHash = Animator.StringToHash("DodgingForward");
    private readonly int DodgingRightHash = Animator.StringToHash("DodgingRight");

    private const float AnimatorDampTime = .1f;
    private const float CrossfadeDuration = .1f;

    Vector2 dodgeDirection;
    float dodgeDuration;


    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgeDirection) : base(stateMachine)
    {
        this.dodgeDirection = dodgeDirection;
    }

    public override void Enter()
    {
        dodgeDuration = stateMachine.DodgeDuration;

        stateMachine.Health.SetInvulnerable(true);

        stateMachine.Animator.SetFloat(DodgingForwardHash, dodgeDirection.y);
        stateMachine.Animator.SetFloat(DodgingRightHash, dodgeDirection.x);

        stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTreeHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgeDirection.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgeDirection.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        Move(movement, deltaTime);
        FaceTarget();

        dodgeDuration = Mathf.Max(dodgeDuration - deltaTime, 0f);

        if (dodgeDuration <= 0f)
            ReturnToLocomotion();
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}

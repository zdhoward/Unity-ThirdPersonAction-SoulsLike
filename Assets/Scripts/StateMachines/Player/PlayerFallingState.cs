using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    readonly int FallHash = Animator.StringToHash("Fall");
    const float CrossfadeDuration = .1f;

    Vector3 momentum;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
            ReturnToLocomotion();

        FaceTarget();
    }

    public override void Exit()
    {
    }
}

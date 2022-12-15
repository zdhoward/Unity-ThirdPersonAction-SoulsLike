using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    readonly int BlockHash = Animator.StringToHash("Block");

    const float AnimatorDampTime = .1f;
    const float CrossfadeDuration = .1f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (!stateMachine.InputHandler.IsBlocking)
            ReturnToLocomotion();
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}

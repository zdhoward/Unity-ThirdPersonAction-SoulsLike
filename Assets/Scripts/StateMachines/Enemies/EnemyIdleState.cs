using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("LocomotionBlendTree");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float AnimatorDampTime = .1f;
    private const float CrossfadeDuration = .1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }
}

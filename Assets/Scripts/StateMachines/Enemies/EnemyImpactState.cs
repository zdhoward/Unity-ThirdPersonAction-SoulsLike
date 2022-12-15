using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    readonly int ImpactHash = Animator.StringToHash("Impact1");

    const float AnimatorDampTime = .1f;
    const float CrossfadeDuration = .1f;

    float duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}

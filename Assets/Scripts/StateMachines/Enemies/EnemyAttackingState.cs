using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int EnemyAttackHash = Animator.StringToHash("EnemyAttack1");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float AnimatorDampTime = .1f;
    private const float CrossfadeDuration = .1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Weapon.SetDamage(stateMachine.AttackDamage, stateMachine.AttackKnockback);

        stateMachine.Animator.CrossFadeInFixedTime(EnemyAttackHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();

        if (GetNormalizedTime(stateMachine.Animator, "Attack") >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));

        }
    }

    public override void Exit()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    float previousFrameTime;

    Attack attack;

    bool hasAppliedForce;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex = 0) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.WeaponDamage.SetDamage(attack.Damage, attack.Knockback);
    }

    public override void Tick(float deltaTime)
    {

        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputHandler.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }

    void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1)
            return;

        if (normalizedTime < attack.ComboAttackTime)
            return;

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    void TryApplyForce()
    {
        if (hasAppliedForce)
            return;

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        hasAppliedForce = true;
    }
}

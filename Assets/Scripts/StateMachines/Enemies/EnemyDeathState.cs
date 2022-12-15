using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    //readonly int DeathHash = Animator.StringToHash("Death");

    // const float AnimatorDampTime = .1f;
    // const float CrossfadeDuration = .1f;

    public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Ragdoll.SetRagdoll(true);
        stateMachine.Weapon.gameObject.SetActive(false);

        GameObject.Destroy(stateMachine.Target);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
    }
}

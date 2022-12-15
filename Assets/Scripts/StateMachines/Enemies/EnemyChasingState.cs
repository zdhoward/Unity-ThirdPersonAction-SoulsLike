using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("LocomotionBlendTree");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float AnimatorDampTime = .1f;
    private const float CrossfadeDuration = .1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        //Move(deltaTime);

        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}

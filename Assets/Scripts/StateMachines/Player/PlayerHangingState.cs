using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    readonly int HangingHash = Animator.StringToHash("Hanging");
    const float CrossfadeDuration = .1f;

    Vector3 ledgeForward;
    Vector3 closestPoint;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.Controller.enabled = false;
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.Controller.enabled = true;

        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputHandler.MovementValue.y < 0f)
        {
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
        else if (stateMachine.InputHandler.MovementValue.y > 0f)
        {
            stateMachine.SwitchState(new PlayerPullupState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}

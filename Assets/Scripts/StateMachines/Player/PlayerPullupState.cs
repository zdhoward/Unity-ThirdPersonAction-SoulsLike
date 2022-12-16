using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullupState : PlayerBaseState
{
    readonly int PullupHash = Animator.StringToHash("Pullup");
    const float CrossfadeDuration = .1f;

    readonly Vector3 ledgeOffset = new Vector3(0f, 2.325f, 0.65f);

    public PlayerPullupState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullupHash, CrossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f)
            return;

        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(ledgeOffset, Space.Self);
        stateMachine.Controller.enabled = true;

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}

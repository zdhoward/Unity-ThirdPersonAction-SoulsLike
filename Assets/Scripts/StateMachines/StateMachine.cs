using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    State currentState;

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public void SwitchState(State state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
}

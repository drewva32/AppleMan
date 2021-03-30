using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
        //Debug.Log(CurrentState);
    }

    public void Init(IState defaultState)
    {
        CurrentState = defaultState;
        CurrentState.OnEnter();
    }
}

using UnityEngine;

public class PluggableStateMachine
{
    public State CurrentState { get; private set; }
    private PluggableStateController _pluggableStateController;

    public PluggableStateMachine(PluggableStateController controller)
    {
        _pluggableStateController = controller;
    }

    public void Init(State startingState)
    {
        CurrentState = startingState;
        Debug.Log("starting state" + CurrentState);
        CurrentState.OnEnter(_pluggableStateController);
    }

    public void ChangeState(State newState)
    {
        CurrentState.OnExit(_pluggableStateController);
     
        newState.OnEnter(_pluggableStateController);
        CurrentState = newState;
        Debug.Log("Entered " + CurrentState);
    }
}

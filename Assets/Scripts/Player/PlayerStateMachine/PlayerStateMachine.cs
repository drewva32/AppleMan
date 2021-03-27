using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    private readonly Player _player;

    public PlayerStateMachine(Player player)
    {
        _player = player;
    }
    public PlayerState CurrentState { get; private set; }
    public PlayerState PreviousState { get; private set; }
    
    
    public void EnterDefaultState(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
        
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == CurrentState)
            return;
        
        CurrentState.Exit();
        PreviousState = CurrentState;
        CurrentState = newState;
        CurrentState.Enter();
    }
}

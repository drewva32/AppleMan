using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerIdleState.asset", menuName = "Scriptable Objects/PlayerIdleStateSO")]

public class PlayerIdleState : PlayerGroundedState
{
    [SerializeField] private PlayerMoveState moveState;
    [SerializeField] private PlayerCrouchIdleState crouchIdleState;
    
    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        
        allTransitions.Add(new StateTransition(this, moveState, () => xInput != 0 && !isExitingState));
        allTransitions.Add(new StateTransition(this, crouchIdleState, () => yInput == -1));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
        // Debug.Log(availableTransitions.Count);
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // if (xInput != 0 && !isExitingState)
        // {
        //     stateMachine.ChangeState(player.MoveState);
        // }
        if (isExitingState)
            return;
        
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }
}

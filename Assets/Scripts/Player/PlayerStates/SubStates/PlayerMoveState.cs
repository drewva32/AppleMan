using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveState.asset", menuName = "Scriptable Objects/PlayerMoveStateSO")]

public class PlayerMoveState : PlayerGroundedState
{
    [SerializeField] private PlayerIdleState idleState;
    [SerializeField] private PlayerCrouchMoveState crouchMoveState;
    

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        
        allTransitions.Add(new StateTransition(this, idleState, () => xInput == 0 && !isExitingState));
        allTransitions.Add(new StateTransition(this, crouchMoveState, () => yInput == -1));
        
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //maybe move isExitingState to this line for early exit.
        
        player.CheckIfShouldFlip(xInput);
        if (isExitingState)
            return;

        // if (xInput == 0 && !isExitingState)
        // {
        //     stateMachine.ChangeState(player.IdleState);
        // }
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
        if (isExitingState)
            return;
        player.SetVelocityX(playerData.movementVelocity * xInput);

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }
}

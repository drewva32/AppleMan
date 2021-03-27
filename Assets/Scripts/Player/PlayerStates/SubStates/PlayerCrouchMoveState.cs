using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerCrouchMoveState.asset", menuName = "Scriptable Objects/PlayerCrouchMoveStateSO")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    [SerializeField] private PlayerCrouchIdleState crouchIdleState;
    [SerializeField] private PlayerMoveState moveState;
    
    
    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this, crouchIdleState, () => xInput == 0));
        allTransitions.Add(new StateTransition(this, moveState, () => yInput != -1 && !isTouchingCeiling));
        
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState)
            return;
        
        player.SetVelocityX(playerData.crouchMovementVelocity * player.FacingDirection);
        player.CheckIfShouldFlip(xInput);
        
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        player.SetColliderHeight(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(false);
    }
}

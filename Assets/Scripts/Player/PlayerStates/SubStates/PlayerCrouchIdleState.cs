using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerCrouchIdleState.asset", menuName = "Scriptable Objects/PlayerCrouchIdleStateSO")]
public class PlayerCrouchIdleState : PlayerGroundedState
{
    [SerializeField] private PlayerCrouchMoveState crouchMoveState;
    [SerializeField] private PlayerIdleState idleState;
    
    
    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        
        allTransitions.Add(new StateTransition(this, crouchMoveState, () => xInput != 0));
        allTransitions.Add(new StateTransition(this, idleState, () => yInput != -1 && !isTouchingCeiling));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.SetColliderHeight(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (isExitingState)
            return;
        player.SetVelocityZero();
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        
    }
}

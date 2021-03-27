using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLandState.asset", menuName = "Scriptable Objects/PlayerLandStateSO")]
public class PlayerLandState : PlayerGroundedState
{
    [SerializeField] private PlayerMoveState moveState;
    [SerializeField] private PlayerIdleState idleState;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this,moveState, () => xInput != 0));
        allTransitions.Add(new StateTransition(this,idleState, () => isAnimationFinished));
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (isExitingState)
            return;
        // if(xInput != 0)
        //     stateMachine.ChangeState(moveState);
        // else if (isAnimationFinished)
        // {
        //     stateMachine.ChangeState(idleState);
        // }
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        
        
        player.SetVelocityX(0);
    }
}

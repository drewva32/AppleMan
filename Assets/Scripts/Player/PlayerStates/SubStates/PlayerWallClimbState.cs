using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWallClimbState.asset", menuName = "Scriptable Objects/PlayerWallClimbStateSo")]
public class PlayerWallClimbState : PlayerTouchingWallState
{
    [SerializeField] private PlayerWallGrabState wallGrabState;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this,wallGrabState, () => yInput != 1));
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
        
        player.SetVelocityY(playerData.wallClimbVelocity);
        
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        // if (yInput != 1)
        // {
        //     stateMachine.ChangeState(wallGrabState);
        // }
    }
}

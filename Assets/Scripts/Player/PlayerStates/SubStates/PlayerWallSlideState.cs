using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWallSlideState.asset", menuName = "Scriptable Objects/PlayerWallSlideStateSO")]
public class PlayerWallSlideState : PlayerTouchingWallState
{
    [SerializeField] private PlayerWallGrabState wallGrabState;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this,wallGrabState, () => grabInput && yInput == 0 || (grabInput && isGrounded)));
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
        
        player.SetVelocityY(-playerData.wallSlideVelocity);
        
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        // if (grabInput && yInput == 0 || (grabInput && isGrounded))
        // {
        //     stateMachine.ChangeState(wallGrabState);
        // }
    }
}

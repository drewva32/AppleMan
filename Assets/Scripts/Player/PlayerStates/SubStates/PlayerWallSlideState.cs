using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWallSlideState.asset", menuName = "Scriptable Objects/PlayerWallSlideStateSO")]
public class PlayerWallSlideState : PlayerTouchingWallState
{
    [SerializeField] private PlayerWallGrabState wallGrabState;
    [SerializeField] private PlayerGroundSlideState groundSlideState;

    private bool _dashInput;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        // allTransitions.Add(new StateTransition(this,wallGrabState, () => grabInput && yInput == 0 || (grabInput && isGrounded)));
        allTransitions.Add(new StateTransition(this, groundSlideState, () => _dashInput && groundSlideState.CheckIfCanSlide()));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();
        if (player.HasAudioManager) 
            AudioManager.Instance.PlayerAudioController.PlayWallSlideSound();

        _dashInput = player.InputHandler.DashInput;
        groundSlideState.ResetCanSlide();
    }

    public override void Exit()
    {
        base.Exit();
        if (player.HasAudioManager) 
            AudioManager.Instance.PlayerAudioController.CancelLoopingSound();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState)
            return;

        _dashInput = player.InputHandler.DashInput;
        
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

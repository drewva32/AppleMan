using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWallGrabState.asset", menuName = "Scriptable Objects/PlayerWallGrabStateSo")]
public class PlayerWallGrabState : PlayerTouchingWallState
{
    [SerializeField] private PlayerWallClimbState wallClimbState;
    [SerializeField] private PlayerWallSlideState wallSlideState;
    
    private Vector2 _positionToHold;


    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this, wallClimbState, () => yInput > 0));
        allTransitions.Add(new StateTransition(this, wallSlideState, () => (yInput < 0 || !grabInput) && !isGrounded));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();
        //player.JumpState.ResetAmountOfJumpsLeft();
        _positionToHold = player.transform.position;
        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (isExitingState)
            return;
        
        HoldPosition();
        
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        // if (yInput > 0)
        // {
        //     stateMachine.ChangeState(wallClimbState);
        // }
        // else if ((yInput < 0 || !grabInput) && !isGrounded)
        // {
        //     stateMachine.ChangeState(wallSlideState);
        // }
    }

    private void HoldPosition()
    {
        player.transform.position = _positionToHold;
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}

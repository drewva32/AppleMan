using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    [SerializeField] private PlayerWallJumpState wallJumpState;
    [SerializeField] private PlayerIdleState idleState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] private PlayerJumpState jumpState;
    
    
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected int xInput;
    protected int yInput;
    protected bool jumpInput;


    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this,wallJumpState, () =>  jumpInput));
        allTransitions.Add(new StateTransition(this,idleState, () =>  isGrounded && !grabInput));
        allTransitions.Add(new StateTransition(this,inAirState, () =>  !isTouchingWall || xInput == player.FacingDirection * -1));
        //|| (xInput != player.FacingDirection && !grabInput)
    }

    public override void Enter()
    {
        base.Enter();
        jumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;
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
        // if (jumpInput)
        // {
        //     wallJumpState.DetermineWallJumpDirection(isTouchingWall);
        //     stateMachine.ChangeState(wallJumpState);
        // }
        // else if (isGrounded && !grabInput)
        // {
        //     stateMachine.ChangeState(idleState);
        // }
        // else if (!isTouchingWall || (xInput != player.FacingDirection && !grabInput))
        // {
        //     stateMachine.ChangeState(inAirState);
        // }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
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

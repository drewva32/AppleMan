using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    [SerializeField] private PlayerJumpState jumpState;
    [SerializeField] private PlayerWallGrabState wallGrabState;
    [SerializeField] private PlayerInAirState inAirState;
    // [SerializeField] private PlayerDashState dashState;
    [SerializeField] private PlayerPunchState punchState;
    [SerializeField] private PlayerGroundSlideState groundSlideState;
    
    

    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    private bool _jumpInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _grabInput;
    private bool _dashInput;
    private bool _punchInput;
    

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        
        allTransitions.Add(new StateTransition(this,jumpState, () => _jumpInput && jumpState.CanJump() && !isTouchingCeiling));
        allTransitions.Add(new StateTransition(this, inAirState, () => !_isGrounded));
        allTransitions.Add(new StateTransition(this, wallGrabState, () => _isTouchingWall && _grabInput));
        // allTransitions.Add(new StateTransition(this,dashState, () => _dashInput && dashState.CheckIfCanDash()));
        allTransitions.Add(new StateTransition(this,punchState, () => _punchInput && punchState.CanPunch && !isTouchingCeiling));
        allTransitions.Add(new StateTransition(this,groundSlideState, () => _dashInput && groundSlideState.CheckIfCanSlide()&& !isTouchingCeiling));

        // foreach (var transition in allTransitions)
        // {
        //     if(pluggedStates.Contains(transition.To))
        //         availableTransitions.Add(transition);
        // }
    }
    
    

    public override void Enter()
    {
        base.Enter();
        
        jumpState.ResetAmountOfJumpsLeft();
        // dashState.ResetCanDash();
        groundSlideState.ResetCanSlide();
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
        _jumpInput = player.InputHandler.JumpInput;
        _grabInput = player.InputHandler.GrabInput;
        _dashInput = player.InputHandler.DashInput;
        _punchInput = player.InputHandler.PunchInput;
        
        // if(_dashInput)
        //     Debug.Log(dashState._lastDashTime);
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                if(!_isGrounded)
                    inAirState.StartCoyoteTime();
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        // if (_jumpInput && player.JumpState.CanJump())
        // {
        //     stateMachine.ChangeState(player.JumpState);
        // }
        // else if (!_isGrounded)
        // {
        //     player.InAirState.StartCoyoteTime();
        //     stateMachine.ChangeState(player.InAirState);
        // }
        // else if (_isTouchingWall && _grabInput)
        // {
        //     stateMachine.ChangeState(player.WallGrabState);
        // }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
        isTouchingCeiling = player.CheckForCeiling();

        if (punchState.CheckPunchCoolDown())
        {
            punchState.ResetCanPunch();
        }
        
        
    }
}

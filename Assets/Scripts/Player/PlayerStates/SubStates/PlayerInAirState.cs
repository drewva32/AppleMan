using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInAirState.asset", menuName = "Scriptable Objects/PlayerInAirStateSO")]
public class PlayerInAirState : PlayerState
{
    [SerializeField] private PlayerLandState landState;
    [SerializeField] private PlayerWallJumpState wallJumpState;
    [SerializeField] private PlayerJumpState jumpState;
    // [SerializeField] private PlayerWallGrabState wallGrabState;
    [SerializeField] private PlayerWallSlideState wallSlideState;
    // [SerializeField] private PlayerLedgeClimbState ledgeClimbState;
    // [SerializeField] private PlayerDashState dashState;
    [SerializeField] private PlayerGroundedState groundedState;
    [SerializeField] private PlayerPunchState punchState;
    
    private int _xInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _dashInput;
    private bool _punchInput;
    private bool _coyoteTime;
    private bool _wallJumpCoyoteTime;
    private bool _isJumping;
    private bool _grabInput;
    private float _startWallJumpCoyoteTime;
    private bool _isTouchingLedge;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        
        allTransitions.Add(new StateTransition(this, landState, () => _isGrounded && player.CurrentVelocity.y < 0.01f));
        // allTransitions.Add(new StateTransition(this, ledgeClimbState, () => _isTouchingWall && !_isTouchingLedge));
        // allTransitions.Add(new StateTransition(this, wallJumpState, () => _jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime)));
        allTransitions.Add(new StateTransition(this,jumpState, () => _jumpInput && jumpState.CanJump()));
        // allTransitions.Add(new StateTransition(this,wallGrabState, () => _isTouchingWall && _grabInput));
        allTransitions.Add(new StateTransition(this,wallSlideState, () => _isTouchingWall && _xInput == player.FacingDirection && player.CurrentVelocity.y < 0));
        // allTransitions.Add(new StateTransition(this,dashState, () => _dashInput && dashState.CheckIfCanDash()));
        allTransitions.Add(new StateTransition(this,punchState, () => _punchInput && punchState.CanPunch));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();
        // if(player.StateMachine.PreviousState == groundedState)
        //     StartCoyoteTime();
    }

    public override void Exit()
    {
        base.Exit();

        _coyoteTime = false;
        oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
        _isTouchingWall = false;
        _isTouchingWallBack = false;
        
        StopWallJumpCoyoteTime();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        
        _xInput = player.InputHandler.NormInputX;
        _jumpInput = player.InputHandler.JumpInput;
        _jumpInputStop = player.InputHandler.JumpInputStop;
        _grabInput = player.InputHandler.GrabInput;
        _dashInput = player.InputHandler.DashInput;
        _punchInput = player.InputHandler.PunchInput;

        CheckJumpCancelMultiplier();
        
        // if (_isGrounded && player.CurrentVelocity.y < 0.01f)
        // {
        //     stateMachine.ChangeState(player.LandState);
        // }
        // else if (_jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime))
        // {
        //     StopWallJumpCoyoteTime();
        //     _isTouchingWall = player.CheckIfTouchingWall();
        //     player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
        //     stateMachine.ChangeState(player.WallJumpState);
        // }
        // else if (_jumpInput && player.JumpState.CanJump())
        // {
        //     stateMachine.ChangeState(player.JumpState);
        // }
        // else if (_isTouchingWall && _grabInput)
        // {
        //     stateMachine.ChangeState(player.WallGrabState);
        // }
        // else if (_isTouchingWall && _xInput == player.FacingDirection && player.CurrentVelocity.y < 0)
        // {
        //     stateMachine.ChangeState(player.WallSlideState);
        // }
        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
        
        player.CheckIfShouldFlip(_xInput);
        player.SetVelocityX(playerData.movementVelocity * _xInput);

        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        //I need to create better animations for jumping. kinds of animations for jumping, rising, and peak of jump
        //animation for falling
        //set x velocity if i want animation for jumping with horizontal movement
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
    public override void DoChecks()
    {
        base.DoChecks();
        oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;
        
        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
        _isTouchingWallBack = player.CheckIfTouchingWallBack();
        _isTouchingLedge = player.CheckIfTouchingLedge();

        // if (_isTouchingWall && !_isTouchingLedge)
        // {
        //     ledgeClimbState.SetDetectedPosition(player.transform.position);
        // }
        
        if (punchState.CheckPunchCoolDown())
        {
            punchState.ResetCanPunch();
        }

        if (!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack &&
            (oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    private void CheckJumpCancelMultiplier()
    {
        if (!_isJumping) return;
        if (_jumpInputStop)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.jumpCancelMultiplier);
            //swapped this line for faster jump falling like hollow knight.
            // player.SetVelocityY(0f);
            _isJumping = false;
        }
        else if(player.CurrentVelocity.y <= 0f) 
            _isJumping = false;

    }

    private void CheckCoyoteTime()
    {
        if (_coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            _coyoteTime = false;
            jumpState.RemoveAllRemainingJumps();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (_wallJumpCoyoteTime && Time.time > _startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            _wallJumpCoyoteTime = false;
            jumpState.RemoveAllRemainingJumps();
        }
    }
    
    public void StartCoyoteTime() => _coyoteTime = true;
    public void StartWallJumpCoyoteTime()
    {
        _wallJumpCoyoteTime = true;
        _startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;
    public void SetIsJumping() => _isJumping = true;
}

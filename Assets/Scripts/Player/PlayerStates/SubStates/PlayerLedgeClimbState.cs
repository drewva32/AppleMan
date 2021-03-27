using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLedgeClimbState.asset", menuName = "Scriptable Objects/PlayerLedgeClimbStateSO")]
public class PlayerLedgeClimbState : PlayerState
{
    [SerializeField] private PlayerInAirState inAirState;
    [SerializeField] private PlayerIdleState idleState;
    [SerializeField] private PlayerWallJumpState wallJumpState;
    [SerializeField] private PlayerJumpState jumpState;
    [SerializeField] private PlayerCrouchIdleState crouchIdleState;
    
    
    
    private Vector2 _detectedPos;
    private Vector2 _cornerPos;
    private Vector2 _startPos;
    private Vector2 _stopPos;

    private bool _isHanging;
    private bool _isClimbing;
    private bool _isTouchingCeiling;

    private int _xInput;
    private int _yInput;
    private bool _jumpInput;
    private int _facingDirection;
    private static readonly int ClimbLedge = Animator.StringToHash("climbLedge");

    public void SetDetectedPosition(Vector2 pos) => _detectedPos = pos;


    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this,idleState, () => isAnimationFinished));
        allTransitions.Add(new StateTransition(this, wallJumpState, () => _jumpInput && !_isClimbing));
        allTransitions.Add(new StateTransition(this, inAirState, () => (_yInput == -1 || _xInput == -_facingDirection) && _isHanging && !_isClimbing));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        jumpState.ResetAmountOfJumpsLeft();
        _facingDirection = player.FacingDirection;
        _isHanging = false;
        _isClimbing = false;
        player.SetVelocityZero();
        player.transform.position = _detectedPos;
        _cornerPos = player.DetermineCornerPosition();
        _startPos.Set(_cornerPos.x - (player.FacingDirection * playerData.startOffset.x), _cornerPos.y - playerData.startOffset.y);
        _stopPos.Set(_cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), _cornerPos.y + playerData.stopOffset.y);

        player.transform.position = _startPos;
        //good for swapping straight to climbing ledge to idle when climbing wall.
        // if (stateMachine.PreviousState == inAirState)
        // {
        //     _isClimbing = true;
        //     player.Anim.SetBool(ClimbLedge,true);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        _isHanging = false;

        if (_isClimbing)
        {
            player.transform.position = _stopPos;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // _facingDirection = player.FacingDirection;
        
        
        if (isAnimationFinished)
        {
            if (_isTouchingCeiling)
                stateMachine.ChangeState(crouchIdleState);
            else
                stateMachine.ChangeState(idleState);
        }
        else
        {
            _xInput = player.InputHandler.NormInputX;
            _yInput = player.InputHandler.NormInputY;
            _jumpInput = player.InputHandler.JumpInput;
        
            player.SetVelocityZero();
            player.transform.position = _startPos;
            
            
            if ((_xInput == player.FacingDirection || _yInput == 1) && _isHanging && !_isClimbing)
            {
                CheckForSpace();
                _isClimbing = true;
                player.Anim.SetBool(ClimbLedge,true);
            }
            else if (_jumpInput && !_isClimbing)
            {
                stateMachine.ChangeState(wallJumpState);
            }
            else if((_yInput == -1 || _xInput == -_facingDirection) && _isHanging && !_isClimbing)
            {
                stateMachine.ChangeState(inAirState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        _isHanging = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool(ClimbLedge, false);
    }

    private void CheckForSpace()
    {
        _isTouchingCeiling =
            Physics2D.Raycast(_cornerPos + (Vector2.up * 0.015f) + (Vector2.right * _facingDirection * 0.015f),
                Vector2.up, playerData.normalColliderSize.y, playerData.whatIsGround);
    }
}

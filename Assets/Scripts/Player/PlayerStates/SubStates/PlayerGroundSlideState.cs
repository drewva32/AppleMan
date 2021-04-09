using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGroundSlideState.asset", menuName = "Scriptable Objects/PlayerGroundSlideStateSO")]

public class PlayerGroundSlideState : PlayerAbilityState
{
    [SerializeField] private PlayerWallSlideState wallSlideState;
    [SerializeField] private PlayerJumpState jumpState;
    
    public bool CanSlide { get; private set; }
    private float _lastSlideTime;
    private bool _touchingWall;
    private bool _isWallBreakable;
    private int _xInput;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        // allTransitions.Add(new StateTransition(this,wallSlideState, () =>  _touchingWall && !_isGrounded));

        _lastSlideTime = 0;
    }

    public override void Enter()
    {
        base.Enter();
        CanSlide = false;
        player.InputHandler.UseDashInput();
        player.SetColliderHeight(true);
        
        if(player.HasAudioManager)
            AudioManager.Instance.PlayerAudioController.PlayGroundSlideSound();

        if (player.StateMachine.PreviousState == wallSlideState)
        {
            player.Flip();
            jumpState.DecreaseAmountOfJumpsLeft();
        }

        player.PlayerHealthController.CanTakeDamage = false;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.Kick();
    }


    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(false);
        player.PlayerHealthController.CanTakeDamage = true;
    }

    public override void LogicUpdate()
    {
        _touchingWall = player.CheckIfTouchingWall();
        _isWallBreakable = player.CheckIfBreakable();
        
        base.LogicUpdate();
        if (_touchingWall && !_isWallBreakable && !_isGrounded)
        {
            player.StateMachine.ChangeState(wallSlideState);
        }
        if (Time.time >= startTime + playerData.maxGroundSlideTime)
        {
            isAbilityDone = true;
            _lastSlideTime = Time.time;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocity(playerData.groundSlideVelocity, Vector2.right * (player.FacingDirection));
    }
    
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public bool CheckIfCanSlide() => CanSlide && Time.time >= _lastSlideTime + playerData.slideCooldown;

    public void ResetCanSlide() => CanSlide = true;
}

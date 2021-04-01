using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGroundSlideState.asset", menuName = "Scriptable Objects/PlayerGroundSlideStateSO")]

public class PlayerGroundSlideState : PlayerAbilityState
{
    public bool CanSlide { get; private set; }
    private float _lastSlideTime;
    private int _xInput;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
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
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + playerData.maxGroundSlideTime)
        {
            isAbilityDone = true;
            _lastSlideTime = Time.time;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocity(playerData.groundSlideVelocity, Vector2.right * player.FacingDirection);
    }
    
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public bool CheckIfCanSlide() => CanSlide && Time.time >= _lastSlideTime + playerData.slideCooldown;

    public void ResetCanSlide() => CanSlide = true;
}

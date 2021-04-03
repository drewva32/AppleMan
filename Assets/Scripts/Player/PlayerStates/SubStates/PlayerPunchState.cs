using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPunchState.asset", menuName = "Scriptable Objects/PlayerPunchStateSO")]
public class PlayerPunchState : PlayerAbilityState
{
    public bool CanPunch { get; private set; }

    private float _lastPunchTime;
    private int _xInput;

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        _lastPunchTime = 0;
        CanPunch = true;
    }

    public override void Enter()
    {
        base.Enter();
        CanPunch = false;
        _lastPunchTime = Time.time;
        player.InputHandler.UsePunchInput();

        if (player.HasAudioManager)
        {
            AudioManager.Instance.PlayerAudioController.PlayPunchSound();
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.Punch();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = player.InputHandler.NormInputX;
        
        if (Time.time >= startTime + playerData.punchTime)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // if(Math.Abs(_xInput - player.FacingDirection) < 0.15)
        //     player.SetVelocityX(playerData.movementVelocity * player.FacingDirection);
        player.CheckIfShouldFlip(_xInput);
        player.SetVelocityX(playerData.movementVelocity * _xInput);
        
    }

    // public bool CheckIfCanPunch()
    // {
    //     return CanPunch;
    // }

    public bool CheckPunchCoolDown()
    {
        return Time.time >= _lastPunchTime + playerData.punchCoolDown;
    }

    public void ResetCanPunch() => CanPunch = true;
}

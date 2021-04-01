using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpState.asset", menuName = "Scriptable Objects/PlayerJumpStateSO")]
public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;
    
    
    //initialize jumps amount
   // _amountOfJumpsLeft = playerData.amountOfJumps;

   public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
       HashSet<PlayerState> pluggedStates)
   {
       base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
       _amountOfJumpsLeft = playerData.amountOfJumps;
   }

   public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        player.SetVelocityY(playerData.jumpVelocity);
        DecreaseAmountOfJumpsLeft();
        inAirState.SetIsJumping();
        isAbilityDone = true;
        
        if(player.HasAudioManager)
            AudioManager.Instance.PlayerAudioController.PlayJumpUpSound();
    }

    public bool CanJump() => _amountOfJumpsLeft > 0;

    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft()
    {
        _amountOfJumpsLeft--;
    }

    public void RemoveAllRemainingJumps()
    {
        _amountOfJumpsLeft = 0;
    }

    public void AddAmountOfJumpsLeft(int jumps) => _amountOfJumpsLeft = jumps;
}

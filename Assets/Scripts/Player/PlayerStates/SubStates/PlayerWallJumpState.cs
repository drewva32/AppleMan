using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWallJumpState.asset", menuName = "Scriptable Objects/PlayerWallJumpStateSO")]
public class PlayerWallJumpState : PlayerAbilityState
{
    [SerializeField] private PlayerJumpState jumpState;
    
    private int wallJumpDirection;

    public override void Enter()
    {
        base.Enter();

        DetermineWallJumpDirection(player.CheckIfTouchingWall());
        player.InputHandler.UseJumpInput();
        jumpState.ResetAmountOfJumpsLeft();
        player.SetVelocity(playerData.wallJumpVelocity,playerData.wallJumpAngle,wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        jumpState.DecreaseAmountOfJumpsLeft();  
        
        if(player.HasAudioManager)
            AudioManager.Instance.PlayerAudioController.PlayJumpUpSound();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        //set x velocity for jumping

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            inAirState.SetIsJumping();
            isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        wallJumpDirection = isTouchingWall ? -player.FacingDirection : player.FacingDirection;
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDashState.asset", menuName = "Scriptable Objects/PlayerDashStateSO")]
public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    
    private float _lastDashTime;
    private bool _isHolding;
    private bool _dashInputStop;
    private Vector2 _dashDirection;
    private Vector2 _dashDirectionInput;
    private Vector2 _lastAfterImagePosition;
    private SpriteRenderer _dashIndicator;
    

    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        _dashIndicator = player.DashDirectionIndicator.GetComponent<SpriteRenderer>();
        _lastDashTime = 0;
    }

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.InputHandler.UseDashInput();
        _dashIndicator.enabled = true;

        _isHolding = true;
        _dashDirection = Vector2.right * player.FacingDirection;
        Debug.Log(_dashDirectionInput.normalized);
        

        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (isExitingState)
            return;

        if (_isHolding)
        {
            _dashDirectionInput = player.InputHandler.DashDirectionInput;
            _dashInputStop = player.InputHandler.DashInputStop;

            if (_dashDirectionInput != Vector2.zero)
            {
                _dashDirection = _dashDirectionInput;
                _dashDirection.Normalize();
            }

            float angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
            player.DashDirectionIndicator.rotation = Quaternion.Euler(0,0,angle + 225);

            if (_dashInputStop || Time.unscaledTime >= startTime + playerData.maxDashHoldTime)
            {
                _isHolding = false;
                _dashIndicator.enabled = false;
                Time.timeScale = 1f;
                startTime = Time.time;
                player.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x));
                player.RB.drag = playerData.drag;
                player.SetVelocity(playerData.dashVelocity, _dashDirection.normalized);
            }
        }
        else
        {
            player.SetVelocity(playerData.dashVelocity, _dashDirection);

            if (Time.time >= startTime + playerData.dashTime)
            {
                player.RB.drag = 0;
                isAbilityDone = true;
                _lastDashTime = Time.time;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    private void PlaceAfterImage()
    {
        
    }
    
    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= _lastDashTime + playerData.dashCoolDown;
    }

    public void ResetCanDash() => CanDash = true;

}

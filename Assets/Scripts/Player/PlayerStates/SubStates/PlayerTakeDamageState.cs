using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerTakeDamageState.asset", menuName = "Scriptable Objects/PlayerTakeDamageStateSO")]
public class PlayerTakeDamageState : PlayerState
{
    [SerializeField] private PlayerIdleState idleState;
    [SerializeField] private PlayerInAirState inAirState;
    
    private bool _isAbilityDone;
    private bool _isGrounded;
    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);
        allTransitions.Add(new StateTransition(this, idleState, () => _isAbilityDone && _isGrounded && player.CurrentVelocity.y < 0.01f));
        allTransitions.Add(new StateTransition(this, inAirState, () => _isAbilityDone));
        foreach (var transition in allTransitions)
        {
            if(pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }

        _isAbilityDone = false;
    }

    public override void Enter()
    {
        // Debug.Log("entered damage state");
        base.Enter();
        player.PlayerHealthController.ResetTookDamage();
        
        if(player.HasAudioManager)
            AudioManager.Instance.PlayerAudioController.PlayHurtSound();
    }

    public override void Exit()
    {
        _isAbilityDone = false;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _isGrounded = player.CheckIfGrounded();
        if(_isGrounded)
            player.SetVelocityZero();
        
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
    }

    public override void AnimationFinishTrigger()
    {
        _isAbilityDone = true;
    }
}

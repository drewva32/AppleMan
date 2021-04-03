using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    [SerializeField] private PlayerIdleState idleState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] private PlayerCrouchIdleState crouchIdleState;

    protected bool isAbilityDone;

    private bool _isGrounded;
    private bool _isTouchingCeiling;


    public override void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        HashSet<PlayerState> pluggedStates)
    {
        base.InitializeState(player, playerStateMachine, playerData, pluggedStates);

        allTransitions.Add(new StateTransition(this, crouchIdleState,
            () => isAbilityDone && _isGrounded && player.CurrentVelocity.y < 0.01f && _isTouchingCeiling));
        allTransitions.Add(new StateTransition(this, idleState,
            () => isAbilityDone && _isGrounded && player.CurrentVelocity.y < 0.01f));
        allTransitions.Add(new StateTransition(this, inAirState, () => isAbilityDone));
        foreach (var transition in allTransitions)
        {
            if (pluggedStates.Contains(transition.To))
                availableTransitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        foreach (var transition in availableTransitions)
        {
            if (transition.Condition())
            {
                stateMachine.ChangeState(transition.To);
                break;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = player.CheckIfGrounded();
        _isTouchingCeiling = player.CheckForCeiling();
    }
}
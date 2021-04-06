using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IState
{
    private EnemyStateController _StateController;
    private bool _hasTakenDamage;
    private bool _isAnimationDone;

    public string AnimationName => "attack";

    public EnemyAttackState(EnemyStateController controller)
    {
        _StateController = controller;
    }

    public void FixedLogicUpdate()
    {
        
    }

    public void LogicUpdate()
    {
        if (_isAnimationDone)
            _StateController.StateMachine.ChangeState(_StateController.EnemyWalkState);
    }

    public void OnEnter()
    {
        _isAnimationDone = false;
        _StateController.Animator.SetBool(AnimationName, true);
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
    }

    public void OnAnimationEnd()
    {
        _isAnimationDone = true;
    }

    public void TakeHit()
    {
        _hasTakenDamage = true;
    }
}

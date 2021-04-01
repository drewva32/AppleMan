using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeHitState : IState
{
    private EnemyStateController _StateController;
    public string AnimationName => "takeHit";
    private bool _isAnimationDone;

    public EnemyTakeHitState(EnemyStateController controller)
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
    { }
}

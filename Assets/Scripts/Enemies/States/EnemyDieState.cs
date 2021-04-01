using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IState
{
    private EnemyStateController _StateController;
    public string AnimationName => "die";

    public EnemyDieState(EnemyStateController controller)
    {
        _StateController = controller;
    }

    public void FixedLogicUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void LogicUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        _StateController.Animator.SetBool(AnimationName, true);
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
    }

    public void OnAnimationEnd()
    {
    }

    public void TakeHit()
    { }
}

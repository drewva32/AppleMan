using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProtectState : IState
{
    public string AnimationName => "protect";
    private EnemyStateController _StateController;

    public EnemyProtectState(EnemyStateController controller)
    {
        _StateController = controller;
    }

    public void FixedLogicUpdate()
    {
    }

    public void LogicUpdate()
    {
    }

    public void OnAnimationEnd()
    {
    }

    public void OnEnter()
    {
        _StateController.Animator.SetBool(AnimationName, true);
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
    }

    public void TakeHit()
    {
    }
}

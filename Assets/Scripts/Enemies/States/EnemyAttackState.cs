using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IState
{
    private EnemyStateController _StateController;
    private bool _hasTakenDamage;

    public string AnimationName => "attack";

    public EnemyAttackState(EnemyStateController controller)
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
        throw new System.NotImplementedException();
    }

    public void OnAnimationEnd()
    {
        throw new System.NotImplementedException();
    }

    public void TakeHit()
    {
        _hasTakenDamage = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IState
{
    private EnemyStateController _StateController;
    public string AnimationName => "chase";

    public EnemyChaseState(EnemyStateController controller)
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
        _StateController.Walkingcontorller.ChangeSpeed();
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
        _StateController.Walkingcontorller.ChangeSpeed();
    }
}

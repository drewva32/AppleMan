using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeHitState : IState
{
    private CarrotStateController _StateController;
    public string AnimationName => "takeHit";

    public EnemyTakeHitState(CarrotStateController controller)
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
}

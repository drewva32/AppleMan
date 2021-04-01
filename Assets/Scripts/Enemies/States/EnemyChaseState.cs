using UnityEngine;

public class EnemyChaseState : IState
{
    private EnemyStateController _StateController;
    private bool _hasTakenDamage;

    public string AnimationName => "chase";

    public EnemyChaseState(EnemyStateController controller)
    {
        _StateController = controller;
    }

    public void FixedLogicUpdate()
    {
        _StateController.Walkingcontorller.Walk();
    }

    public void LogicUpdate()
    {
        if (Vector3.Distance(_StateController.transform.position, _StateController.Player.position) > _StateController.Walkingcontorller.ChaseDistance)
        {
            _StateController.StateMachine.ChangeState(_StateController.EnemyWalkState);
        }
        _StateController.Walkingcontorller.CheckDirection();
    }

    public void OnEnter()
    {
        _StateController.Animator.SetBool(AnimationName, true);
        _StateController.Walkingcontorller.ChangeSpeed();
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
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

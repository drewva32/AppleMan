using UnityEngine;

public class EnemyWalkState : IState
{
    private EnemyStateController _StateController;
    public string AnimationName => "walk";
    private bool _canSeePlayer = false;

    public EnemyWalkState(EnemyStateController controller)
    {
        _StateController = controller;
    }

    public void FixedLogicUpdate()
    {
        _StateController.Walkingcontorller.Walk();
    }

    public void LogicUpdate()
    {
        _canSeePlayer = Physics2D.Raycast(_StateController.Walkingcontorller.WallCheck.position, new Vector3(-_StateController.Walkingcontorller.transform.localScale.x, 0, 0), _StateController.Walkingcontorller.ChaseDistance, _StateController.Walkingcontorller.PlayerLayer);
        if (_canSeePlayer)
        {
            _StateController.StateMachine.ChangeState(_StateController.EnemyChaseState);
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
}

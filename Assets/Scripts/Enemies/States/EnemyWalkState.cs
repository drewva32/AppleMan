using UnityEngine;

public class EnemyWalkState : IState
{
    private EnemyStateController _StateController;
    public string AnimationName => "walk";
    private bool _canSeePlayer = false;
    private bool _hasTakenDamage;

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

        if(_hasTakenDamage)
        {
            _StateController.StateMachine.ChangeState(_StateController.EnemyTakeHitState);
        }
        else if(_canSeePlayer)
        {
            // TODO: Change this to ChaseState
            //_StateController.StateMachine.ChangeState(_StateController.EnemyChaseState);
            _StateController.TakeSlide(0, _StateController.Walkingcontorller._player.position - _StateController.transform.position);
        }
        _StateController.Walkingcontorller.CheckDirection();
    }

    public void OnEnter()
    {
        _hasTakenDamage = false;
        _StateController.Animator.SetBool(AnimationName, true);
        _StateController.Walkingcontorller.ChangeSpeed();
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
    }

    public void OnAnimationEnd()
    {

    }

    public void TakeHit()
    {
        _hasTakenDamage = true;
    }
}

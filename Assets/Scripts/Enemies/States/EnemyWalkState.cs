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
        _canSeePlayer = Physics2D.Raycast(_StateController.Walkingcontorller.WallCheck.position, _StateController.Walkingcontorller.vectorDirection, _StateController.Walkingcontorller.ChaseDistance, _StateController.Walkingcontorller.PlayerLayer);

        if (_hasTakenDamage)
        {
            _StateController.StateMachine.ChangeState(_StateController.EnemyTakeHitState);
        }
        else if(_canSeePlayer)
        {
            // TODO: Change this to ChaseState
            switch (_StateController.Walkingcontorller.UseAttackState)
            {
                case 1:
                    _StateController.StateMachine.ChangeState(_StateController.EnemyAttackState);
                    break;
                case 2:
                    _StateController.StateMachine.ChangeState(_StateController.EnemyChaseState);
                    break;
                case 3:
                    _StateController.StateMachine.ChangeState(_StateController.EnemyProtectState);
                    break;
                default:
                    _StateController.StateMachine.ChangeState(_StateController.EnemyWalkState);
                    break;
            }
        }
        _StateController.Walkingcontorller.CheckDirection();
    }

    public void OnEnter()
    {
        _hasTakenDamage = false;
        _StateController.Animator.SetBool(AnimationName, true);
        // _StateController.Walkingcontorller.ChangeSpeed();
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

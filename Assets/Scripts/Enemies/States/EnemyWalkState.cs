using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : IState
{
    private CarrotStateController _StateController;
    public string AnimationName => "walk";

    public EnemyWalkState(CarrotStateController controller)
    {
        _StateController = controller;
    }

    public void FixedLogicUpdate()
    {
        // Moves the rigidbody
        _StateController.Walkingcontorller.Walk();
    }

    public void LogicUpdate()
    {
        // This is the logic where the enemy takes in surroundings
        
        // Change to chase state
        if(Vector3.Distance(_StateController.transform.position, _StateController.Player.position) < _StateController.Walkingcontorller.ChaseDistance)
        {
            _StateController.StateMachine.ChangeState(_StateController.EnemyChaseState);
        }
        _StateController.Walkingcontorller.CheckDirection();
    }

    public void OnEnter()
    {
        _StateController.Animator.SetBool(AnimationName, true);
    }

    public void OnExit()
    {
        _StateController.Animator.SetBool(AnimationName, false);
    }
}

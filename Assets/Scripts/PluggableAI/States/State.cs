using UnityEngine;

public class State : ScriptableObject
{
    [SerializeField] public string _animBoolName;
    [SerializeField] private StateAction[] _actions;
    [SerializeField] private Transition[] _transitions;
    
    public virtual void OnEnter(PluggableStateController controller)
    {
        controller.Animator.SetBool(_animBoolName,true);
        controller.SetStartTime(Time.time);
        controller.Enemy.AllowDamage(true);
    }

    public void DoBasicActions(PluggableStateController controller)
    {
        CheckTransitions(controller);
        
        for (int i = 0; i < _actions.Length; i++)
        {
            _actions[i].LogicUpdate(controller);
        }
    }

    public void DoPhysicsActions(PluggableStateController controller)
    {
        for (int i = 0; i < _actions.Length; i++)
        {
            _actions[i].FixedLogicUpdate(controller);
        }
    }

    public void CheckTransitions(PluggableStateController controller)
    {
        foreach (var transition in _transitions)
        {
            if (transition.Decision.Condition(controller))
            {
                controller.StateMachine.ChangeState(transition.TransitionToState);
            }
        }
    }

    public virtual void OnExit(PluggableStateController controller)
    {
        controller.Animator.SetBool(_animBoolName, false);
    }


}

using System;
using UnityEngine;

public class PluggableStateController : MonoBehaviour
{
    [SerializeField] private State _startingState;
    public Animator Animator => _animator;
    public EnemyBase Enemy => _enemy;
    public PluggableStateMachine StateMachine { get; private set; }
    public WalkingController WalkingController { get; private set; }
    public float StartTime { get; private set; }
    public bool IsAnimationComplete{ get; private set; }
    public bool IsAnimationTriggered{ get; private set; }
    
    private Animator _animator;
    private EnemyBase _enemy;
    
    
    private void Awake()
    {
        _enemy = GetComponent<EnemyBase>();
        _animator = GetComponentInChildren<Animator>();
        StateMachine = new PluggableStateMachine(this);
        WalkingController = GetComponent<WalkingController>();
    }

    private void Start() => StateMachine.Init(_startingState);
    private void Update() => StateMachine.CurrentState.DoBasicActions(this);
    private void FixedUpdate() => StateMachine.CurrentState.DoPhysicsActions(this);

    public void SetStartTime(float time) => StartTime = time;

    public void OnAnimationEvent() => IsAnimationTriggered = true;
    public void ResetAnimationEventTriggered() => IsAnimationTriggered = false;

    public void OnAnimationEnd() => IsAnimationComplete = true;
    public void ResetAnimationComplete() => IsAnimationComplete = false;
}

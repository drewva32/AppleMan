using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : ScriptableObject
{
    [SerializeField]private string animBoolName;
    [SerializeField] private PlayerTakeDamageState playerTookDamageState;
    
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected List<StateTransition> allTransitions = new List<StateTransition>();
    public List<StateTransition> availableTransitions = new List<StateTransition>();
    protected Dictionary<Type, PlayerState> statesDic = new Dictionary<Type, PlayerState>();

    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected bool tookDamage;

    protected float startTime;
    
    

    public virtual void InitializeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, HashSet<PlayerState> pluggedStates)
    {
        this.player = player;
        this.stateMachine = playerStateMachine;
        this.playerData = playerData;
        allTransitions.Add(new StateTransition(this, playerTookDamageState, () => tookDamage));
    }

    public virtual void InitializeTransitionList (HashSet<PlayerState> pluggedStates)
    {
        
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName,true);
        // Debug.Log(animBoolName);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName,false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        DoChecks();
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void DoChecks()
    {
        tookDamage = player.PlayerHealthController.TookDamage;
    }

    public virtual void AnimationTrigger()
    {
        
    }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
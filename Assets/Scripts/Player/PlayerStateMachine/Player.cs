using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    [Tooltip("Stats such as move speed, jump height etc.")]
    [SerializeField] private PlayerData playerData;
    
    [Header("Check Positions")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform ceilingCheck;
    [Header("Pluggable State Machine")]
    [SerializeField] private PlayerState[] pluggableStates;
    
    public PlayerStateMachine StateMachine { get; private set; }
    // public PlayerIdleState IdleState { get; private set; }
    // public PlayerMoveState MoveState { get; private set; }
    // public PlayerJumpState JumpState { get; private set; }
    // public PlayerInAirState InAirState { get; private set; }
    // public PlayerLandState LandState { get; private set; }
    // public PlayerWallSlideState WallSlideState { get; private set; }
    // public PlayerWallGrabState WallGrabState { get; private set; }
    // public PlayerWallClimbState WallClimbState { get; private set; }
    // public PlayerWallJumpState WallJumpState { get; private set; }

    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
     public Rigidbody2D RB { get; private set; }
     public CapsuleCollider2D MovementCollider { get; private set; }
     public Transform DashDirectionIndicator { get; private set; }


    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 _workSpace;
    private List<PlayerState> _states = new List<PlayerState>();

    private HashSet<PlayerState> _pluggedStates = new HashSet<PlayerState>();


    private void Awake()
    {
        StateMachine = new PlayerStateMachine(this);//added constructor just for debugging with public current State
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<CapsuleCollider2D>();
        InputHandler = GetComponent<PlayerInputHandler>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        

        
        // IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        // MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        // JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        // InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        // LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        // WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        // WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        // WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        // WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");

        foreach (var state in pluggableStates)
        {
            if (state == null)
                continue;
            _pluggedStates.Add(state);
        }

        foreach (var state in pluggableStates)
        {
            if (state == null)
                continue;
            state.InitializeState(this, StateMachine, playerData,_pluggedStates);
        }
        
        
    }

    private void Start()
    {
        FacingDirection = 1;
        StateMachine.EnterDefaultState(pluggableStates[0]);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        if (CurrentVelocity.y < playerData.terminalVelocity)
            SetVelocityY(playerData.terminalVelocity);
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = _workSpace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        _workSpace = direction * velocity;
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocityX(float velocity)
    {
        _workSpace.Set(velocity, CurrentVelocity.y);
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }

    public void SetVelocityY(float velocity)
    {
        _workSpace.Set(CurrentVelocity.x,velocity);
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }
    
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }

   
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * playerData.wallCheckDistance);
    //     Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * -playerData.wallCheckDistance);
    // }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void SetColliderHeight(bool crouching)
    {
        // Vector2 center = MovementCollider.offset;
        // _workSpace.Set(MovementCollider.size.x, height);
        //
        // center.y += (height - MovementCollider.size.y) / 2;
        //
        // MovementCollider.size = _workSpace;
        // MovementCollider.offset = center;
        if (crouching)
        {
            MovementCollider.direction = CapsuleDirection2D.Horizontal;
            MovementCollider.offset = playerData.crouchColliderOffset;
            MovementCollider.size = playerData.crouchColliderSize;
        }
        else
        {
            MovementCollider.direction = CapsuleDirection2D.Vertical;
            MovementCollider.offset = playerData.normalColliderOffset;
            MovementCollider.size = playerData.normalColliderSize;
        }
        
    }
    
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection,
            playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        _workSpace.Set((xDist + 0.015f) * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(_workSpace), Vector2.down,
            ledgeCheck.position.y - wallCheck.position.y + 0.015f, playerData.whatIsGround);
        float yDist = yHit.distance;
        
        _workSpace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
        return _workSpace;
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position,playerData.groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallCheck.position,Vector2.right * FacingDirection* playerData.wallCheckDistance);
        Gizmos.DrawWireSphere(ceilingCheck.position,playerData.groundCheckRadius);
    }
}

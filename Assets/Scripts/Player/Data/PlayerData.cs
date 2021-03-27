using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Terminal Velocity")] public float terminalVelocity = -15f;
    [Header("Move State")] public float movementVelocity = 10f;
    [Header("Jump State")] public float jumpVelocity = 20f;
    public int amountOfJumps = 2;

    [Header("In Air State")] public float coyoteTime = 0.2f;
    public float jumpCancelMultiplier = 0.5f;

    [Header("Wall Jump State")] public float wallJumpVelocity = 20;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Wall Slide State")] public float wallSlideVelocity = 3f;

    [Header("Wall Climb State")] public float wallClimbVelocity = 3f;

    [Header("Ledge Climb State")] public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Dash State")] public float dashCoolDown = 0.5f;
    public float maxDashHoldTime = 1;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("Punch State")] public float punchCoolDown = 0.3f;
    public float punchTime = 0.25f;
    public float punchDamage = 2;

    [Header("GroundSlide State")] public float maxGroundSlideTime = .75f;
    public float slideCooldown = 1f;
    public float groundSlideVelocity = 12f;

    [Header("Crouch States")] public float crouchMovementVelocity = 4f;
    public Vector2 crouchColliderOffset = new Vector2(0,0.8f);
    public Vector2 crouchColliderSize = new Vector2(0.8f, .35f);
    public Vector2 normalColliderOffset = new Vector2(0, 1.02f);
    public Vector2 normalColliderSize = new Vector2(0.6f, 0.76f);
    
    [Header("Check Variables")] public float wallCheckDistance = 0.5f;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
   
}

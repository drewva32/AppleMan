using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private float jumpHoldTime = 0.2f;
    [SerializeField] private float dashHoldTime = 0.2f;

    
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool PunchInput { get; private set; }

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private PlayerInput playerInput;
    private Camera _cam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        
        
        NormInputX = (int) (RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int) (RawMovementInput * Vector2.up).normalized.y;
        // NormInputX = Mathf.CeilToInt((RawMovementInput * Vector2.right).normalized.x);
        // NormInputX = Mathf.CeilToInt((RawMovementInput * Vector2.right).normalized.x);

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInputStop = false;
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
            GrabInput = true;
        if (context.canceled)
            GrabInput = false;
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = _cam.ScreenToWorldPoint((Vector3) RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    public void UsePunchInput() => PunchInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + jumpHoldTime) 
            JumpInput = false;
    }
    
    
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if(context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnPunchInput(InputAction.CallbackContext context)
    {
        if (context.started)
            PunchInput = true;
        if (context.canceled)
            PunchInput = false;
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + dashHoldTime)
        {
            DashInput = false;
        }
    }

    
    
    
}

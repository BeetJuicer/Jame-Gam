using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;
    private PlayerScript player;

    //--//
    public Vector2 moveDirection { get; private set; }
    //-//
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
    public bool InteractInput { get; private set; }
    public bool SubmitInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool EscapeInput { get; private set; }

    [SerializeField]
    private float inputHoldtime = 0.2f;

    private float jumpInputStarttime;
    private float dashInputStarttime;

    private bool jumpInputHold;

    private static PlayerInputHandler instance;

    private void Awake()
    {
        /*
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }*/
        instance = this;
    }

    public static PlayerInputHandler GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<PlayerScript>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        
            if (Mathf.Abs(RawMovementInput.x) > 0.5f)
            {
                NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
            }
            else
            {
                NormInputX = 0;
            }

            if (Mathf.Abs(RawMovementInput.y) > 0.5f)
            {
                NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
            }
            else
            {
                NormInputY = 0;
            }

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStarttime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }
    
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }
    
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStarttime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)(RawDashDirectionInput)) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //if (player.CheckIfGrounded())
        //{
            if (context.started)
            {
                InteractInput = true;
            }

            if (context.canceled)
            {
                InteractInput = false;
            }
        //}
    }

    public void OnSubmitInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SubmitInput = true;
        }
        else if (context.canceled)
        {
            SubmitInput = false;
        }
    }
    
    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireInput = true;
        }
        else if (context.canceled)
        {
            FireInput = false;
        }
    }
    
    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EscapeInput = true;
        }

        if (context.canceled)
        {
            EscapeInput = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    public void UseSubmitInput() => SubmitInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStarttime + inputHoldtime)
        {
            JumpInput = false;
        }
    }
    
    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStarttime + inputHoldtime)
        {
            DashInput = false;
        }
    }

    //----------------------------------------------//

    public Vector2 GetMoveDirection()
    {
        moveDirection = new Vector2(NormInputX, NormInputY);
        return moveDirection;
    }

    public bool GetInteractPressed()
    {
        bool result = InteractInput;
        InteractInput = false;
        return result;
    }
    
    public bool GetEscapePressed()
    {
        bool result = EscapeInput;
        EscapeInput = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = SubmitInput;
        SubmitInput = false;
        return result;
    }

    public bool GetFirePressed()
    {
        bool result = FireInput;
        FireInput = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        SubmitInput = false;
    }
}

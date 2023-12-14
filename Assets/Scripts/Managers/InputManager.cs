using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; internal set; }

    private GameInput Inputs { get; set; }

    [HideInInspector] public UnityEvent<Vector2> Player_Move;
    [HideInInspector] public UnityEvent<bool> Player_Jump;
    [HideInInspector] public UnityEvent<bool> Player_Crouch;
    [HideInInspector] public UnityEvent<bool> Player_Interact;
    [HideInInspector] public UnityEvent<bool> Player_Pause;

    public void OnEnable()
    {
        if (Instance)
            return;

        EnableControls();
    }

    public void OnDisable()
    {
        if (Instance)
            return;
            
        DisableControls();
    }

    public void EnableControls()
    {
        Inputs.Player.Move.performed += PlayerMove;
        Inputs.Player.Move.started += PlayerMove;
        Inputs.Player.Move.canceled += PlayerMove;

        Inputs.Player.Jump.performed += PlayerJump;
        Inputs.Player.Crouch.performed += PlayerCrouch;
        Inputs.Player.Interact.performed += PlayerInteract;
        Inputs.Player.Pause.performed += PlayerPause;

        Inputs.Player.Enable();
    }

    public void DisableControls()
    {
        Inputs.Player.Move.performed -= PlayerMove;
        Inputs.Player.Move.started -= PlayerMove;
        Inputs.Player.Move.canceled -= PlayerMove; 

        Inputs.Player.Jump.performed -= PlayerJump;
        Inputs.Player.Crouch.performed -= PlayerCrouch;
        Inputs.Player.Interact.performed -= PlayerInteract;
        Inputs.Player.Pause.performed -= PlayerPause;

        Inputs.Player.Disable();
    }

    private void Awake()
    {
        Instance ??= this;

        Inputs = new();
        Player_Move ??= new();
        Player_Jump ??= new();
        Player_Crouch ??= new();
        Player_Interact ??= new();
        Player_Pause ??= new();
    }

    public void PlayerMove(InputAction.CallbackContext ctx) => Player_Move?.Invoke(ctx.ReadValue<Vector2>());
    public void PlayerJump(InputAction.CallbackContext ctx) => Player_Jump?.Invoke(ctx.ReadValueAsButton());
    public void PlayerCrouch(InputAction.CallbackContext ctx) => Player_Crouch?.Invoke(ctx.ReadValueAsButton());
    public void PlayerInteract(InputAction.CallbackContext ctx) => Player_Interact?.Invoke(ctx.ReadValueAsButton());
    public void PlayerPause(InputAction.CallbackContext ctx) => Player_Pause?.Invoke(ctx.ReadValueAsButton());
}

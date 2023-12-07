using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; internal set; }

    private GameInput Inputs { get; set; }

    [HideInInspector] public UnityEvent<Vector3> Player_Move;
    [HideInInspector] public UnityEvent<bool> Player_Jump;
    [HideInInspector] public UnityEvent<bool> Player_Crouch;
    [HideInInspector] public UnityEvent<bool> Player_Interact;
    [HideInInspector] public UnityEvent<bool> Player_Pause;

    public void EnableControls() => OnEnable();
    public void DisableControls() => OnDisable();

    public void PlayerMove(InputAction.CallbackContext ctx) => Player_Move?.Invoke(ctx.ReadValue<Vector3>());
    public void PlayerJump(InputAction.CallbackContext ctx) => Player_Jump?.Invoke(ctx.ReadValue<bool>());
    public void PlayerCrouch(InputAction.CallbackContext ctx) => Player_Crouch?.Invoke(ctx.ReadValue<bool>());
    public void PlayerInteract(InputAction.CallbackContext ctx) => Player_Interact?.Invoke(ctx.ReadValue<bool>());
    public void PlayerPause(InputAction.CallbackContext ctx) => Player_Pause?.Invoke(ctx.ReadValue<bool>());

    private void OnEnable()
    {
        Inputs.Player.Move.performed += PlayerMove;
        Inputs.Player.Jump.performed += PlayerJump;
        Inputs.Player.Crouch.performed += PlayerCrouch;
        Inputs.Player.Interact.performed += PlayerInteract;
        Inputs.Player.Pause.performed += PlayerPause;
    }

    private void OnDisable()
    {
        Inputs.Player.Move.performed -= PlayerMove;
        Inputs.Player.Jump.performed -= PlayerJump;
        Inputs.Player.Crouch.performed -= PlayerCrouch;
        Inputs.Player.Interact.performed -= PlayerInteract;
        Inputs.Player.Pause.performed -= PlayerPause;
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
}

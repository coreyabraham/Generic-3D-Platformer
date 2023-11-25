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

    private void OnEnable()
    {
        // Inputs.Player.Move.performed += Player_Move;
    }

    private void OnDisable()
    {
        // Inputs.Player.Move.performed -= Player_Move;
    }

    private void Start()
    {
        Inputs = new();
        Player_Move ??= new();
        Player_Jump ??= new();
        Player_Crouch ??= new();
        Player_Interact ??= new();
        Player_Pause ??= new();
    }

    private void Awake() => Instance ??= this;

    public void EnableControls() => OnEnable();
    public void DisableControls() => OnDisable();

    public void PlayerMove(InputAction.CallbackContext ctx) => Player_Move?.Invoke(ctx.ReadValue<Vector2>());
}

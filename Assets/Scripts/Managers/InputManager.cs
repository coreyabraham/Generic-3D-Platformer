using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handle all Player-Based Inputs.
/// [ Uses: GameInput.cs ]
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Input Manager : Instance / Singleton
    /// </summary>
    public static InputManager Instance { get; internal set; }

    private GameInput Inputs { get; set; }

    // Create Event Definitions.
    [HideInInspector] public UnityEvent<Vector2> Player_Move;
    [HideInInspector] public UnityEvent<bool> Player_Jump;
    [HideInInspector] public UnityEvent<bool> Player_Pause;

    /// <summary>
    /// Enables the controls when the script is enabled.
    /// </summary>
    public void OnEnable()
    {
        if (Instance)
            return;

        EnableControls();
    }

    /// <summary>
    /// Disables the controls when the script is disabled.
    /// </summary>
    public void OnDisable()
    {
        if (Instance)
            return;
            
        DisableControls();
    }

    /// <summary>
    /// Attaches all event controls to their respective methods and enables the controls.
    /// </summary>
    public void EnableControls()
    {
        Inputs.Player.Move.performed += PlayerMove;
        Inputs.Player.Move.started += PlayerMove;
        Inputs.Player.Move.canceled += PlayerMove;

        Inputs.Player.Jump.performed += PlayerJump;
        Inputs.Player.Pause.performed += PlayerPause;

        Inputs.Player.Enable();
    }

    /// <summary>
    /// Removes all event controls form their assigned methods and disables the controls.
    /// </summary>
    public void DisableControls()
    {
        Inputs.Player.Move.performed -= PlayerMove;
        Inputs.Player.Move.started -= PlayerMove;
        Inputs.Player.Move.canceled -= PlayerMove;

        Inputs.Player.Jump.performed -= PlayerJump;
        Inputs.Player.Pause.performed -= PlayerPause;

        Inputs.Player.Disable();
    }

    /// <summary>
    /// Initalize all Events / Instances.
    /// </summary>
    private void Awake()
    {
        Instance ??= this;
        Inputs = new();

        Player_Move ??= new();
        Player_Jump ??= new();
        Player_Pause ??= new();
    }

    /// <summary>
    /// Invoke the "Player_Move" Event whenever the InputAction.CallbackContext is fired.
    /// </summary>
    /// <param name="ctx"></param>
    public void PlayerMove(InputAction.CallbackContext ctx) => Player_Move?.Invoke(ctx.ReadValue<Vector2>());

    /// <summary>
    /// Invoke the "Player_Jump" Event whenever the InputAction.CallbackContext is fired.
    /// </summary>
    /// <param name="ctx"></param>
    public void PlayerJump(InputAction.CallbackContext ctx) => Player_Jump?.Invoke(ctx.ReadValueAsButton());

    /// <summary>
    /// Invoke the "Player_Pause" Event whenever the InputAction.CallbackContext is fired.
    /// </summary>
    /// <param name="ctx"></param>
    public void PlayerPause(InputAction.CallbackContext ctx) => Player_Pause?.Invoke(ctx.ReadValueAsButton());
}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; internal set; }

    [field: SerializeField] InputActionAsset Input { get; set; }

    private void Awake() => Instance ??= this;
}

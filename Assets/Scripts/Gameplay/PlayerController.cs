using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A class that associates a PhysicsMaterial with a SoundName string
/// </summary>
[System.Serializable]
public class AudibleMaterial
{
    [field: SerializeField] public string SoundName { get; set; }
    [field: SerializeField] public PhysicMaterial Material { get; set; }
}

/// <summary>
/// Manage general Player-related actions (I.e; Movement, Jumping, Gravity and Animations)
/// [ Requires: CharacterController ]
/// [ Depends on: InputManager.cs, GameManager.cs ]
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Private (Serialized) Definitions

    [field: Header("General")]
    [field: SerializeField] private float CharacterSpeed { get; set; }
    [field: SerializeField] private float JumpHeight { get; set; }
    [field: SerializeField] private float GravityMultiplier { get; set; }
    
    [field: Header("Gound Checking")]
    [field: SerializeField] private GameObject Feet { get; set; }
    [field: SerializeField] private LayerMask GroundMask { get; set; }

    [field: Header("Specific")]
    [field: SerializeField] private GameObject Model { get; set; }
    [field: SerializeField] private AudibleMaterial[] Materials;

    // Private Variables

    private float _gravity = -9.81f;
    private float _velocity;

    private Animator _animator;
    private CharacterController _controller;
    private Rigidbody _rigid;

    private Vector3 _moveDirection;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isDying;
    private float _targetRotation;

    /// <summary>
    /// Checks "Materials" (AudibleMaterial[]) and plays a sound depending on if the Player is grounded or not.
    /// [ Each time the Player's "HumanoidRunning" animation takes a Step, this function is ran. ]
    /// </summary>
    /// <param name="footIndex"></param>
    public void Step(int footIndex)
    {
        PhysicMaterial obj = null;
        RaycastHit hit;

        if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit))
            obj = hit.collider.sharedMaterial;

        // This could be improved... I never got around to doing that
        string footstep = string.Empty;
        if (obj != null)
        {
            foreach (AudibleMaterial mat in Materials)
            {
                if (mat.Material == obj)
                {
                    footstep = mat.SoundName;
                    break;
                }
            }
        }

        //Debug.Log(footstep + "Footstep" + footIndex.ToString());
        AudioManager.Instance.Play(footstep + "Footstep" + footIndex.ToString());
    }

    /// <summary>
    /// Update "_moveDirection" private variable using InputManager.cs' Move Event
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction) => _moveDirection = new Vector3(direction.x, 0, direction.y);

    /// <summary>
    /// Handles Jumping Velocity.
    /// </summary>
    /// <param name="value"></param>
    public void Jump(bool value)
    {
        if (_isDying || _isJumping)
            return;

        _isJumping = true;
        _velocity += Mathf.Sqrt(JumpHeight * -3.0f * _gravity);
    }

    /// <summary>
    /// Handles Death, restarts the level if "LivesCount" is above zero.
    /// [ Uses: SceneController.cs ]
    /// </summary>
    public void TriggerDeath()
    {
        if (_isDying)
            return;

        _isDying = true;

        AudioManager.Instance.Play("Lost Gold");
        InputManager.Instance.DisableControls();

        LifeManager.Instance.ModifyLives(false, LifeTypes.Regular);

        if (SaveFileManager.Instance.SelectedSaveFile.LivesCount > 0)
            SceneController.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Wait for GameManager.cs and InputManager.cs to load before execution.
    /// [ Uses: GameManager, InputManager ]
    /// </summary>
    /// <returns></returns>
    private async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null)
            await Task.Yield();
    }

    /// <summary>
    /// Handles Initalization.
    /// [ Uses: GameManager.cs, InputManager.cs ]
    /// </summary>
    private void OnEnable()
    {
        // Wait for GameManager and InputManager to load
        GameManager.WaitForTask(WaitForManagers(), () => {
            // Initalize all AudibleMaterial members
            foreach (AudibleMaterial mat in Materials)
            {
                if (mat.SoundName == string.Empty)
                    mat.SoundName = mat.Material.name;
            }

            // Enable Controls and Hook Events to PlayerController.cs Methods
            InputManager.Instance.EnableControls();
            InputManager.Instance.Player_Move.AddListener(Move);
            InputManager.Instance.Player_Jump.AddListener(Jump);

            // Assign Private Components
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _rigid = GetComponentInChildren<Rigidbody>();

            // Enable the PlayerUI and Fix "(Clone)" addition on model name
            GameManager.Instance.TogglePlayerUI.Invoke(true);
            name = "Player";
        });
    }

    /// <summary>
    /// Handles Disable Control for the Player.
    /// [ Uses: InputManager.cs ]
    /// </summary>
    private void OnDisable()
    {
        InputManager.Instance.Player_Move.RemoveListener(Move);
        InputManager.Instance.Player_Jump.RemoveListener(Jump);
        InputManager.Instance.DisableControls();
    }

    /// <summary>
    /// Update Player Movement + Animations
    /// </summary>
    private void Update()
    {
        _isRunning = (_moveDirection.x != 0 || _moveDirection.z != 0);
        _isGrounded = Physics.CheckSphere(Feet.transform.position, _controller.radius, GroundMask);
            
        _animator.SetBool("IsRunning", _isRunning && _isGrounded);
        _animator.SetBool("IsJumping", _isJumping && !_isGrounded);

        if (!_animator.GetBool("IsJumping") && _isGrounded)
            _isJumping = false;

        if (_isGrounded && _velocity < 0.0f)
            _velocity = 0.0f;

        float speed = 0.0f;

        if (_isRunning)
        {
            speed = CharacterSpeed;

            _targetRotation = Quaternion.LookRotation(_moveDirection).eulerAngles.y + GameManager.Instance.Camera.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, _targetRotation, 0);

            Model.transform.rotation = Quaternion.Slerp(Model.transform.rotation, rotation, 20 * Time.deltaTime);
        }

        Vector3 refinedDirection = Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward;
        _controller.Move(refinedDirection * speed * Time.deltaTime);

        _velocity += _gravity * GravityMultiplier * Time.deltaTime;
        _controller.Move(new Vector3(0, _velocity, 0) * Time.deltaTime);
    }
}

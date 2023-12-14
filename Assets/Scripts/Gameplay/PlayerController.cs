using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AudibleMaterial
{
    [field: SerializeField] public string SoundName { get; set; }
    [field: SerializeField] public PhysicMaterial Material { get; set; }
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [field: Header("General")]
    [field: SerializeField] private float CharacterSpeed { get; set; }
    [field: SerializeField] private float JumpHeight { get; set; }
    [field: SerializeField] private float GravityMultiplier { get; set; }

    [field: Header("Specific")]
    [field: SerializeField] private GameObject Model { get; set; }
    [field: SerializeField] private PlayerStates PlayerState { get; set; }
    [field: SerializeField] private AudibleMaterial[] Materials;

    private float _gravity = -9.81f;
    [field: SerializeField] private float _velocity;

    private Dictionary<BodyPart, Transform> parts;

    private Animator _animator;
    private CharacterController _controller;
    private Rigidbody _rigid;
    [field: SerializeField] private Vector3 _moveDirection { get; set; } // will become a private once tested properly!
    [field: SerializeField] private bool _isRunning { get; set; }
    [field: SerializeField] private bool _isGrounded { get; set; }
    [field: SerializeField] private float _targetRotation { get; set; }

    public void Step(int footIndex)
    {
        PhysicMaterial obj = null;
        RaycastHit hit;

        if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit))
            obj = hit.collider.sharedMaterial;

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

        AudioManager.Instance.Play(footstep + footIndex.ToString());
    }
    
    private void Gravity()
    {
        if (_isGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
            return;
        }

        _velocity += _gravity * GravityMultiplier * Time.deltaTime;
    }

    private void Rotation()
    {
        if (!_isRunning)
            return;

        _targetRotation = Quaternion.LookRotation(_moveDirection).eulerAngles.y + GameManager.Instance.Camera.transform.rotation.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, _targetRotation, 0);

        Model.transform.rotation = Quaternion.Slerp(Model.transform.rotation, rotation, 20 * Time.deltaTime);
    }

    private void Movement()
    {
        _animator.SetBool("IsRunning", _isRunning);

        if (!_isRunning)
        {
            if (PlayerState != PlayerStates.Crouching || PlayerState != PlayerStates.Dying)
                PlayerState = PlayerStates.Idle;

            return;
        }

        if (PlayerState != PlayerStates.Crouching || PlayerState != PlayerStates.Dying)
            PlayerState = PlayerStates.Running;

        _controller.Move(_moveDirection * Time.deltaTime);
        //Vector3 targetDirection = Quaternion.Euler(0, _targetRotation, 0) * _moveDirection;
    }

    public void Move(Vector2 direction) => _moveDirection = new Vector3(direction.x * CharacterSpeed, _velocity, direction.y * CharacterSpeed);

    public void Jump(bool value)
    {
        if (PlayerState == PlayerStates.Dying || PlayerState == PlayerStates.Crouching)
            return;

        PlayerState = PlayerStates.Jumping;

    }

    public void TriggerDeath()
    {
        if (PlayerState == PlayerStates.Dying)
            return;

        PlayerState = PlayerStates.Dying;
        Debug.LogWarning("DO DEATH HERE!", this);

        AudioManager.Instance.Play("Lost Gold");

        // DO CODE HERE!
    }

    private async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null)
            await Task.Yield();
    }

    private void OnEnable()
    {
        GameManager.WaitForTask(WaitForManagers(), () => {
            foreach (AudibleMaterial mat in Materials)
            {
                if (mat.SoundName == string.Empty)
                    mat.SoundName = mat.Material.name;
            }

            InputManager.Instance.EnableControls();
            InputManager.Instance.Player_Move.AddListener(Move);
            InputManager.Instance.Player_Jump.AddListener(Jump);

            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _rigid = GetComponentInChildren<Rigidbody>();

            GameManager.Instance.TogglePlayerUI.Invoke(true);

            name = "Player";
        });
    }

    private void OnDisable()
    {
        InputManager.Instance.Player_Move.RemoveListener(Move);
        InputManager.Instance.Player_Jump.RemoveListener(Jump);
    }

    private void Update()
    {
        _isRunning = (_moveDirection.x != 0 || _moveDirection.z != 0);
        _isGrounded = _controller.isGrounded;

        Gravity();
        Rotation();
        Movement();
    }
}

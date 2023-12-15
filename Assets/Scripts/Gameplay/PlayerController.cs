using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [field: Header("Gound Checking")]
    [field: SerializeField] private GameObject Feet { get; set; }
    [field: SerializeField] private LayerMask GroundMask { get; set; }

    [field: Header("Specific")]
    [field: SerializeField] private GameObject Model { get; set; }
    [field: SerializeField] private AudibleMaterial[] Materials;

    private float _gravity = -9.81f;
    private float _velocity;

    private Animator _animator;
    private CharacterController _controller;
    private Rigidbody _rigid;

    private Vector3 _moveDirection;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isDying { get; set; }
    private float _targetRotation;

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

        //Debug.Log(footstep + "Footstep" + footIndex.ToString());
        AudioManager.Instance.Play(footstep + "Footstep" + footIndex.ToString());
    }

    public void Move(Vector2 direction) => _moveDirection = new Vector3(direction.x, 0, direction.y);

    public void Jump(bool value)
    {
        if (_isDying || _isJumping)
            return;

        _isJumping = true;
        _velocity += Mathf.Sqrt(JumpHeight * -3.0f * _gravity);
    }

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
        
        //Vector3 targetDirection = Quaternion.Euler(0, _targetRotation, 0) * _moveDirection;
    }
}

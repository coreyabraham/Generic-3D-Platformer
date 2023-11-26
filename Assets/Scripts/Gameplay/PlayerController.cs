using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public float CharacterSpeed { get; set; }
    [field: SerializeField] public float JumpHeight { get; set; }
    [field: SerializeField] public bool IsJumping { get; set; }
    [field: SerializeField] public bool IsMoving { get; set; }
    [field: SerializeField] public bool IsDead { get; set; }

    private Animator _animator;
    [field: SerializeField] public Vector2 _moveDirection { get; set; } // will become a private once tested properly!

    public void Step(int index) => AudioManager.Instance.Play("Footstep" + index.ToString());
    public void Move(Vector2 direction) => _moveDirection = direction;

    public void TriggerDeath()
    {
        if (IsDead)
            return;

        IsDead = true;
        // DO CODE HERE!
    }

    private void OnEnable()
    {
        GameManager.Instance.Player = this;
        InputManager.Instance.Player_Move.AddListener(Move);
    }

    private void Update()
    {
        float fwd = Input.GetAxis("Vertical");

        _animator.SetFloat("Forward", Mathf.Abs(fwd));
        _animator.SetFloat("Sense", Mathf.Sign(fwd));

        _animator.SetFloat("Turn", Input.GetAxis("Horizontal"));
    }
    
    private void Start() { _animator = GetComponent<Animator>(); name = "Player"; }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AudibleMaterial
{
    [field: SerializeField] public string SoundName { get; set; }
    [field: SerializeField] public PhysicMaterial Material { get; set; }
}

public class PlayerController : MonoBehaviour
{
    [field: Header("General")]
    [field: SerializeField] private float CharacterSpeed { get; set; }
    [field: SerializeField] private float JumpHeight { get; set; }

    [field: Header("Specific")]
    [field: SerializeField] private PlayerStates PlayerState { get; set; }
    [field: SerializeField] private AudibleMaterial[] Materials;

    [field: Header("Body Parts"), Tooltip("Unused for now, will be used once VFX is introduced!")]
    [field: SerializeField] private Transform Root;
    [field: SerializeField] private Transform Head;
    [field: SerializeField] private Transform Chest;
    [field: SerializeField] private Transform LeftHand;
    [field: SerializeField] private Transform RightHand;
    [field: SerializeField] private Transform LeftFoot;
    [field: SerializeField] private Transform RightFoot;

    private Dictionary<BodyPart, Transform> parts;

    private Animator _animator;
    private CharacterController _controller;
    private Rigidbody _rigid;
    [field: SerializeField] private Vector3 _moveDirection { get; set; } // will become a private once tested properly!

    public Transform GetBodyPart(BodyPart part)
    {
        if (parts == null)
        {
            parts = new Dictionary<BodyPart, Transform>
            {
                [BodyPart.Root] = transform,
                [BodyPart.Head] = Head,
                [BodyPart.Chest] = Chest,
                [BodyPart.LeftHand] = LeftHand,
                [BodyPart.RightHand] = RightHand,
                [BodyPart.LeftFoot] = LeftFoot,
                [BodyPart.RightFoot] = RightFoot
            };

            if (parts.ContainsKey(part))
                return parts[part];
        }

        return transform;
    }

    public void Step(int footIndex)
    {
        // unused for the moment, will be used for VFX Management!
        //Transform foot = footIndex == 1 ? LeftFoot : RightFoot;

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

    public void Move(Vector3 direction) 
    {
        Debug.Log("MOVING!");
        _moveDirection = direction;
    }

    public void Jump(bool value)
    {
        Debug.Log(value);
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

            InputManager.Instance.Player_Move.AddListener(Move);
            InputManager.Instance.Player_Jump.AddListener(Jump);

            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _rigid = GetComponentInChildren<Rigidbody>();

            GameManager.Instance.UpdatePlayerUI(true);
            name = "Player";
        });
    }

    private void OnDisable()
    {
        InputManager.Instance.Player_Move.RemoveListener(Move);
        InputManager.Instance.Player_Jump.RemoveListener(Jump);
    }

    private bool ran; // test!
    private void Update()
    {
        if (!ran && Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("CLICKED!", this);
            ran = true;
            GoldManager.Instance.ClearGold(1);
        }

        // (ADD THIS IN AS A REPLACEMENT ONCE "InputManager" WORKS PROPERLY!)
        //_controller.Move(_moveDirection);

        // rework this controller to be more free-form and less "tanky" !!!
        float fwd = Input.GetAxis("Vertical");

        _animator.SetFloat("Forward", Mathf.Abs(fwd * CharacterSpeed));
        _animator.SetFloat("Sense", Mathf.Sign(fwd * CharacterSpeed));

        _animator.SetFloat("Turn", Input.GetAxis("Horizontal"));

        if (PlayerState == PlayerStates.Jumping || PlayerState == PlayerStates.Dying)
        {
            if (PlayerState == PlayerStates.Jumping)
            {
                Debug.Log("ACTIVATE PLAYER JUMP HERE!");
            }
            
            return;
        }

        if (fwd != 0)
            PlayerState = PlayerStates.Running;
        else
            PlayerState = PlayerStates.Idle;
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;

    public void Step(int index) => AudioManager.Instance.Play("Footstep" + index.ToString());

    private void Start() => _animator = GetComponent<Animator>();

    private void Update()
    {
        float fwd = Input.GetAxis("Vertical");

        _animator.SetFloat("Forward", Mathf.Abs(fwd));
        _animator.SetFloat("Sense", Mathf.Sign(fwd));

        _animator.SetFloat("Turn", Input.GetAxis("Horizontal"));
    }
}

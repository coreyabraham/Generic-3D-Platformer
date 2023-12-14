using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    // Move this entire script into a "GenericCollider" class inheritence

    [field: SerializeField] public string SoundName { get; set; } = "Level Complete!";
    private bool Triggered;

    public void OnTriggerEnter(Collider other)
    {
        if (Triggered || other.GetComponent<PlayerController>() == null)
            return;

        Triggered = true;

        InputManager.Instance.DisableControls();
        AudioManager.Instance.Play(SoundName);

        GameManager.Instance.LevelFinished.Invoke();
    }
}

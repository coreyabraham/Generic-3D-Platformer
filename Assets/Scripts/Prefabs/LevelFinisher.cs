using UnityEngine;

/// <summary>
/// Handle Level Finishing Logic.
/// [ Uses: GameManager.cs ]
/// </summary>
public class LevelFinisher : MonoBehaviour
{
    // Define all Variables
    [field: SerializeField] public string SoundName { get; set; } = "Level Complete!";
    [field: SerializeField] public string SceneOverride { get; set; }
    private bool Triggered;

    /// <summary>
    /// When Touched, Disable Controls, Play a sound and call LevelFinished Event from GameManager.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (Triggered || other.GetComponent<PlayerController>() == null)
            return;

        Triggered = true;

        InputManager.Instance.DisableControls();
        AudioManager.Instance.Play(SoundName);

        GameManager.Instance.LevelFinished.Invoke(SceneOverride);
    }
}

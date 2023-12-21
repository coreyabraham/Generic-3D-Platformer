using UnityEngine;

/// <summary>
/// Track the Player using the Camera on Startup
/// [ Uses: GameManager.cs ]
/// </summary>
public class TrackCameraToPlayer : MonoBehaviour
{
    // All Definitions (Inspector Visible)
    [field: SerializeField] private Transform Tracker { get; set; }
    [field: SerializeField] private CursorLockMode LockMode { get; set; }
    [field: SerializeField] private bool ShowCursor { get; set; }
    [field: SerializeField] private bool EmulateMobile { get; set; }

    /// <summary>
    /// Update the FieldOfView based on GameManager and Update Cursor logic.
    /// </summary>
    private void Update()
    {
        GameManager.Instance.Camera.m_Lens.FieldOfView = GameManager.Instance.FieldOfView;

        if (EmulateMobile || GameManager.Instance.PlatformType == PlatformTypes.Mobile)
            return;

        Cursor.lockState = LockMode;
        Cursor.visible = ShowCursor;
    }

    /// <summary>
    /// When the player (and in turn this script) starts, track the player's "Tracker" object via the Camera.
    /// </summary>
    private void Start()
    {
        GameManager.Instance.Camera.Follow = Tracker;
        GameManager.Instance.Camera.LookAt = Tracker;
    }
}

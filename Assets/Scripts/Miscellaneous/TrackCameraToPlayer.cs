using UnityEngine;

public class TrackCameraToPlayer : MonoBehaviour
{
    [field: SerializeField] public Transform Tracker { get; set; }
    [field: SerializeField] public CursorLockMode LockMode { get; set; }
    [field: SerializeField] public bool ShowCursor { get; set; }
    [field: SerializeField] public bool EmulateMobile { get; set; }

    private void Update()
    {
        GameManager.Instance.Camera.m_Lens.FieldOfView = GameManager.Instance.FieldOfView;

        if (EmulateMobile || GameManager.Instance.PlatformType == PlatformTypes.Mobile)
            return;

        Cursor.lockState = LockMode;
        Cursor.visible = ShowCursor;
    }

    private void Start()
    {
        GameManager.Instance.Camera.Follow = Tracker;
        GameManager.Instance.Camera.LookAt = Tracker;
    }
}

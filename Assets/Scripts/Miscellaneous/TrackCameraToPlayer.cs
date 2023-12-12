using UnityEngine;

public class TrackCameraToPlayer : MonoBehaviour
{
    [field: SerializeField] public Transform Tracker { get; set; }
    [field: SerializeField] public CursorLockMode LockMode { get; set; }
    [field: SerializeField] public bool ShowCursor { get; set; }

    private void Update()
    {
        Cursor.lockState = LockMode;
        Cursor.visible = ShowCursor;
        
        GameManager.Instance.Camera.m_Lens.FieldOfView = GameManager.Instance.FieldOfView;
    }

    private void Start()
    {
        GameManager.Instance.Camera.Follow = Tracker;
        GameManager.Instance.Camera.LookAt = Tracker;
    }
}

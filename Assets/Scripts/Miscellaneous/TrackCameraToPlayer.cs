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
    }

    private void Start()
    {
        GameManager.Instance.Camera.Follow = Tracker;
        GameManager.Instance.Camera.LookAt = Tracker;
    }
}

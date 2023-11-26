using UnityEngine;

public class TrackCameraToPlayer : MonoBehaviour
{
    [field: SerializeField] public GameObject Tracker { get; set; }

    public void Start()
    {
        GameManager.Instance.Camera.LookAt = Tracker.transform;
        GameManager.Instance.Camera.Follow = Tracker.transform;
    }
}

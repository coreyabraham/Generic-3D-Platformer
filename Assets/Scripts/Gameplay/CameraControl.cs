using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //[field: SerializeField] public Transform CameraTarget { get; set; }
    //[field: SerializeField] public bool TargetPlayerOnStart { get; set; }
    //[field: SerializeField] public Vector2 LerpSpeeds { get; set; } = new(0.02f, 0.01f);

    //[field: SerializeField] public Vector2 Turn { get; set; }
    //[field: SerializeField] public float Sensitivity { get; set; } = 0.5f;

    //public void UpdateMouse()
    //{
    //    Turn = new Vector2(
    //        Turn.x + Input.GetAxis("Mouse X") * Sensitivity,
    //        Turn.y + Input.GetAxis("Mouse Y") * Sensitivity
    //        );

    //    transform.localRotation = Quaternion.Euler(-Turn.y, Turn.x, 0.0f);
    //}

    //private void Update()
    //{
    //    if (TargetPlayerOnStart) // FIGURE OUT A WAY TO DO THIS ONLY ONCE!
    //        CameraTarget = GameManager.Instance.Player.CameraTarget.transform;

    //    UpdateMouse();

    //    // transform.position = Vector3.Lerp(transform.position, CameraTarget.position, LerpSpeeds.x);
    //    // transform.rotation = Quaternion.Lerp(transform.rotation, CameraTarget.rotation, LerpSpeeds.y);
    //}
}

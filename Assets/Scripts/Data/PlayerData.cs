using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; internal set; }

    public Transform SavedTransformation { get; set; }

    private void Awake() => Instance ??= this;
}

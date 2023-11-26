using UnityEngine;

public class SaveFileData : MonoBehaviour
{
    [field: SerializeField] public string FileName { get; set; }
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public string LevelName { get; set; }
    [field: SerializeField] public int GoldCount { get; set; }
    [field: SerializeField] public int LivesCount { get; set; }
    [field: SerializeField] public Transform PreviousTransform { get; set; }
}

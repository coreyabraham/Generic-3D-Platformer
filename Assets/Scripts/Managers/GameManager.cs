using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [field: SerializeField] public Camera Camera { get; set; }
    [field: SerializeField] public PlayerData PlayerData { get; set; }
    public PlayerController Player { get; set; }

    private void Update()
    {

    }

    private void Start()
    {

    }

    private void Awake() => Instance ??= this;
}

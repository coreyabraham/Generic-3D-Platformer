using System;
using System.Threading.Tasks;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [field: SerializeField] public Cinemachine.CinemachineFreeLook Camera { get; set; }
    [field: SerializeField] public SaveFileData CurrentSaveFile { get; set; }
    [field: SerializeField] public PlayerController Player { get; set; }

    public static async void WaitForTask(Task timeoutTask, Action onComplete)
    {
        if (await Task.WhenAny(timeoutTask, Task.Delay(1000)) == timeoutTask)
        {
            onComplete();
            return;
        }

        Debug.LogWarning("Instance of managers not found!");
    }

    public static async Task Timer(int timeMS) => await Task.Delay(timeMS);

    private void Update()
    {
        // CODE GOES HERE!
    }

    private void Start()
    {
        // CODE GOES HERE!
    }

    private void Awake() => Instance ??= this;
}

using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [field: Header("General")]
    [field: SerializeField] public Cinemachine.CinemachineFreeLook Camera { get; set; }
    [field: SerializeField] public PlayerController Player { get; set; }

    [field: Header("Miscellaneous")]
    [field: SerializeField] public float FieldOfView { get; set; } = 40.0f;
    [field: SerializeField] private UnityEvent<bool> TogglePlayerUI { get; set; }

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

    // port this into "Assistant.cs" script full of other one-off functions like this!
    public void UpdatePlayerUI(bool option) => TogglePlayerUI?.Invoke(option);

    private void Awake()
    {
        Instance ??= this;
        TogglePlayerUI ??= new();
    }
}

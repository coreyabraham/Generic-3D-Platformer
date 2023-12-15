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
    [field: SerializeField] public PlatformTypes PlatformType { get; set; }

    [field: Header("Events")]
    [field: SerializeField] public UnityEvent<bool> TogglePlayerUI { get; set; }
    [field: SerializeField] public UnityEvent<string> LevelFinished { get; set; }

    // port this into "Assistant.cs" script full of other one-off functions like this!
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

    private void Awake()
    {
        Instance ??= this;
        TogglePlayerUI ??= new();

        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop: PlatformType = PlatformTypes.PC; break;
            case DeviceType.Handheld: PlatformType = PlatformTypes.Mobile; break;
            case DeviceType.Unknown: PlatformType = PlatformTypes.WebGL; break;
        }

        if (Application.isMobilePlatform && PlatformType != PlatformTypes.Mobile)
            PlatformType = PlatformTypes.Mobile;
    }
}

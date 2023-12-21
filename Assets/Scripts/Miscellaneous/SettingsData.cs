using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// All Saveable Settings Data
/// [ Used By: SettingsData.cs, SettingsUI.cs ]
/// </summary>
[Serializable]
public class BaseSettings
{
    [Header("Audio")]
    public float MasterVolume;
    public float SoundVolume;
    public float MusicVolume;

    [Header("Video")]
    public int DisplayResolution;
    public int GameQuality;
    public int FPS;
    public int CamFOV;
    public bool DisplayFPS;
    public bool Fullscreen;
}

/// <summary>
/// Handle all Savable SettingsData Instancing
/// [ Uses: DataManager.cs ]
/// </summary>
public class SettingsData : MonoBehaviour
{
    /// <summary>
    /// Include BaseSettings Instance Reference
    /// </summary>
    public BaseSettings BaseSettings;

    // All Fields (Both Private and Public)
    [field: Header("Miscellaneous")]
    [field: SerializeField] private string FileName { get; set; } = "SettingsData.json";
    [field: SerializeField] private string FilePath { get; set; }

    [field: Header("Events")]
    [field: SerializeField] public UnityEvent InitalizeEvent { get; set; }
    [field: SerializeField] public UnityEvent<BaseSettings> SettingsChanged { get; set; }

    [field: Header("Debugging")]
    [field: SerializeField] private bool DeleteFileOnStart { get; set; }

    [HideInInspector] public Resolution[] Resolutions;
    [HideInInspector] public BaseSettings DefaultSettings { get; internal set; }

    /// <summary>
    /// Apply Settings using BaseSettings data.
    /// </summary>
    /// <param name="Settings"></param>
    public void ApplySettings(BaseSettings Settings)
    {
        // if (Settings == BaseSettings)
        // {
        //     Debug.LogWarning("No additional changes have been made, skipping...");
        //     return;
        // }

        BaseSettings = Settings;
        DataManager dataInst = new();

        dataInst.Save(BaseSettings, FilePath);
        SettingsChanged?.Invoke(BaseSettings);
    }

    /// <summary>
    /// Set BaseSettings to DefaultSettings to revert all changes.
    /// </summary>
    public void SetDefaults() => BaseSettings = DefaultSettings;

    /// <summary>
    /// Create a file if it doesn't exist, if it exists read from it.
    /// </summary>
    private void ManageData()
    {
        DataManager dataInst = new();

        if (DeleteFileOnStart && File.Exists(FilePath))
        {
            Debug.LogWarning("Settings File Debug Enabled, Deleting: " + FilePath);
            File.Delete(FilePath);
        }

        if (!File.Exists(FilePath))
        {
            Debug.Log(FileName + " has been created in Directory: " + Application.persistentDataPath + "//");
            dataInst.Save(BaseSettings, FilePath);
            
            return;
        }

        string jsonFileContents = File.ReadAllText(FilePath);
        BaseSettings = dataInst.Get(jsonFileContents);
    }

    /// <summary>
    /// Initalize FilePath, DefaultSettings and Resolutions.
    /// </summary>
    private void Start()
    {
        if (FilePath == string.Empty)
            FilePath = Application.persistentDataPath + "//" + FileName;
        else
            FilePath = FilePath + "//" + FileName;

        DefaultSettings = BaseSettings;
        Resolutions = Screen.resolutions;

        ManageData();
        InitalizeEvent?.Invoke();
    }
}

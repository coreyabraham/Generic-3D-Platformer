using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

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

public class SettingsData : MonoBehaviour
{
    public BaseSettings BaseSettings;

    [field: Header("Miscellaneous")]
    [field: SerializeField] public string FileName { get; set; } = "SettingsData.json";
    [field: SerializeField] public string FilePath { get; set; }

    [field: Header("Events")]
    [field: SerializeField] public UnityEvent InitalizeEvent { get; set; }
    [field: SerializeField] public UnityEvent<BaseSettings> SettingsChanged { get; set; }

    [field: Header("Debugging")]
    [field: SerializeField] public bool DeleteFileOnStart { get; set; }

    [HideInInspector] public Resolution[] Resolutions;
    [HideInInspector] public BaseSettings DefaultSettings;

    public void ApplySettings(BaseSettings Settings)
    {
        if (Settings == BaseSettings)
        {
            Debug.LogWarning("No additional changes have been made, skipping...");
            return;
        }

        BaseSettings = Settings;
        DataManager dataInst = new();

        dataInst.Save(BaseSettings, FilePath);
        SettingsChanged?.Invoke(BaseSettings);
    }

    public void SetDefaults() => BaseSettings = DefaultSettings;

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
            Debug.Log(FileName + " has been created in Directory: " + Application.persistentDataPath + "\\");
            dataInst.Save(BaseSettings, FilePath);
            
            return;
        }

        string jsonFileContents = File.ReadAllText(FilePath);
        BaseSettings = dataInst.Get(jsonFileContents);
    }

    private void Start()
    {
        if (FilePath == string.Empty)
            FilePath = Application.persistentDataPath + "\\" + FileName;
        else
            FilePath = FilePath + "\\" + FileName;

        DefaultSettings = BaseSettings;
        Resolutions = Screen.resolutions;

        ManageData();
        InitalizeEvent?.Invoke();
    }
}

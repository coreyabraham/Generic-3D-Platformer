using UnityEngine;
using System.IO;

/// <summary>
/// Manage Json Settings Data.
/// [ Uses: SettingsData.cs ]
/// </summary>
public class DataManager
{
    private static DataManager instance;

    /// <summary>
    /// Data Manager : Instance / Singleton
    /// </summary>
    public static DataManager Instance { get => instance ??= new(); }

    /// <summary>
    /// Save Settings Data to a Json file.
    /// </summary>
    /// <param name="Settings"></param>
    /// <param name="FilePath"></param>
    public void Save(BaseSettings Settings, string FilePath)
    {
        string data = JsonUtility.ToJson(Settings, true);
        File.WriteAllText(FilePath, data);
    }

    /// <summary>
    /// Get Settings Data from a pre-existing Json file.
    /// </summary>
    /// <param name="JsonString"></param>
    /// <returns></returns>
    public BaseSettings Get(string JsonString) => JsonUtility.FromJson<BaseSettings>(JsonString);
}

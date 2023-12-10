using UnityEngine;
using System.IO;

public class DataManager
{
    private static DataManager instance;
    public static DataManager Instance { get => instance ??= new(); }

    // add xml documentation above all functions below!

    public void Save(BaseSettings Settings, string FilePath)
    {
        string data = JsonUtility.ToJson(Settings, true);
        File.WriteAllText(FilePath, data);
    }

    public void Load(BaseSettings Settings, string SavedData) => JsonUtility.FromJsonOverwrite(SavedData, Settings);

    public BaseSettings Get(string JsonString) => JsonUtility.FromJson<BaseSettings>(JsonString);
}

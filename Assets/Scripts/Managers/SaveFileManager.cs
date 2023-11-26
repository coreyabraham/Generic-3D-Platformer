using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager Instance { get; internal set; }

    [field: SerializeField] public string SaveName { get; set; } = "SaveGame"; // REMOVE THIS!
    [field: SerializeField] public string DirectoryName { get; set; } = "Saves";
    [field: SerializeField] public SaveFileData[] SaveFiles { get; set; }
    [field: SerializeField] public int SaveFileTargetIndex { get; set; }

    public SaveFileData LoadFromFile()
    {
        BinaryFormatter formater = new();
        FileStream saveFile = File.Open(DirectoryName + "/" + SaveName + ".bin", FileMode.Open);
        saveFile.Close();
        
        return (SaveFileData)formater.Deserialize(saveFile);
    }

    public void SaveToFile()
    {
        if (!Directory.Exists(DirectoryName))
            Directory.CreateDirectory(DirectoryName);

        BinaryFormatter formater = new();
        FileStream saveFile = File.Create(DirectoryName + "/" + SaveName + ".bin");

        formater.Serialize(saveFile, SaveFiles[SaveFileTargetIndex]);
        saveFile.Close();

        Debug.LogWarning("Successfully Saved To: " + Directory.GetCurrentDirectory().ToString() + "/Saves/" + SaveName + ".bin", this);
    }

    private void Start() => Instance ??= this;
}

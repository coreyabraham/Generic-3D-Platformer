using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager Instance { get; internal set; }

    [field: SerializeField] public string DirectoryName { get; set; } = "Saves";
    [field: SerializeField] public string DirectoryPath { get; set; }
    [field: SerializeField] public string GlobalFileType { get; set; }
    [field: SerializeField] public SaveFileData DefaultSaveFileData { get; set; }
    [field: SerializeField] public SaveFileData[] SaveFiles { get; set; }

    public SaveFileData SelectedSaveFile { get; internal set; }

    public void SetSelectedSaveFile(SaveFileData SaveFile)
    {
        if (SaveFile == null)
        {
            Debug.LogWarning("Provided 'SaveFile' is null, cannot Set 'SelectedSaveFile' with null!", this);
            return;
        }

        SelectedSaveFile = SaveFile;
    }

    public void SetSelectedSaveFile()
    {
        if (SelectedSaveFile != null)
            return;

        SelectedSaveFile = SaveFiles[0];
    }

    private SaveFileData LoadSave(SaveFileData SaveFile)
    {
        BinaryFormatter formater = new();
        
        FileStream saveFile = File.Open(DirectoryPath + "/" + SaveFile.FileName + GlobalFileType, FileMode.Open);
        SaveFileData loadData = (SaveFileData)formater.Deserialize(saveFile);

        saveFile.Close();
        return loadData;
    }

    public SaveFileData LoadFromFile(SaveFileData SaveFile)
    {
        if (SaveFile == null)
        {
            Debug.LogWarning("Provided 'SaveFile' is null, cannot Load from File!", this);
            return null;
        }

        return LoadSave(SaveFile);
    }

    public SaveFileData LoadFromFile()
    {
        if (SelectedSaveFile == null)
        {
            Debug.LogWarning("'SelectedSaveFile' is equal to null, cannot Load from null File!", this);
            return null;
        }

        return LoadSave(SelectedSaveFile);
    }

    private void SaveData(SaveFileData SaveFile)
    {
        BinaryFormatter formater = new();
        FileStream saveFile = File.Create(DirectoryPath + "/" + SaveFile.FileName + ".bin");

        formater.Serialize(saveFile, SaveFile);
        saveFile.Close();

        Debug.LogWarning("Successfully Saved: " + SaveFile.FileName + GlobalFileType + " To: " + DirectoryPath, this);
    }

    public void SaveToFile(SaveFileData SaveFile)
    {
        if (SaveFile == null)
        {
            Debug.LogWarning("Provided 'SaveFile' is null, cannot Save to File!", this);
            return;
        }

        SaveData(SaveFile);
    }

    public void SaveToFile()
    {
        if (SelectedSaveFile == null)
        {
            Debug.LogWarning("Provided, 'SaveFile' is null, cannot Save to a Null File!", this);
            return;
        }

        SaveData(SelectedSaveFile);
    }

    private void Start()
    {
        Instance ??= this;
        
        if (DirectoryPath == string.Empty)
            DirectoryPath = Application.persistentDataPath + "/" + DirectoryName;
        else
            DirectoryPath = DirectoryPath + "/" + DirectoryName;

        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);

        foreach (SaveFileData data in SaveFiles)
        {
            if (!File.Exists(DirectoryPath + "/" + data.FileName + GlobalFileType))
                SaveToFile(data);
        }
    }
}
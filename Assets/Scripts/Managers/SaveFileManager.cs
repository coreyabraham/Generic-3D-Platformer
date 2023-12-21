using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

/// <summary>
/// Handle Save File Management (Game Save Data + Binary Data).
/// </summary>
public class SaveFileManager : MonoBehaviour
{
    /// <summary>
    /// Save File Manager : Instance / Singleton
    /// </summary>
    public static SaveFileManager Instance { get; internal set; }

    // Get Inspector Changes (If any)
    [field: SerializeField] private string DirectoryName { get; set; } = "Saves";
    [field: SerializeField] private string DirectoryPath { get; set; }
    [field: SerializeField] private string GlobalFileType { get; set; }

    // List Save Files (Default or not) for Inspector Usage
    [field: SerializeField] public SaveFileData DefaultSaveFileData { get; set; }
    [field: SerializeField] public SaveFileData[] SaveFiles { get; set; }

    /// <summary>
    /// The currently selected Save File from "SaveFileData[] SaveFiles" (SaveFileManager.cs).
    /// </summary>
    public SaveFileData SelectedSaveFile { get; internal set; }

    /// <summary>
    /// Set the Selected Save File Instance to an independant SaveFileData Instance.
    /// </summary>
    /// <param name="SaveFile"></param>
    public void SetSelectedSaveFile(SaveFileData SaveFile)
    {
        if (SaveFile == null)
        {
            Debug.LogWarning("Provided 'SaveFile' is null, cannot Set 'SelectedSaveFile' with null!", this);
            return;
        }

        SelectedSaveFile = SaveFile;
    }

    /// <summary>
    /// Set the Selected Save File Instance to the default.
    /// </summary>
    public void SetSelectedSaveFile()
    {
        if (SelectedSaveFile != null)
            return;

        SelectedSaveFile = SaveFiles[0];
    }

    /// <summary>
    /// Load Save File Data from it's data file from the DirectoryPath.
    /// </summary>
    /// <param name="SaveFile"></param>
    /// <returns>(SaveFileData) LoadedData</returns>
    private SaveFileData LoadSave(SaveFileData SaveFile)
    {
        BinaryFormatter formater = new();
        
        FileStream saveFile = File.Open(DirectoryPath + "/" + SaveFile.FileName + GlobalFileType, FileMode.Open);
        SaveFileData loadData = (SaveFileData)formater.Deserialize(saveFile);

        saveFile.Close();
        return loadData;
    }

    /// <summary>
    /// Load from a File using a SaveFileData Instance.
    /// </summary>
    /// <param name="SaveFile"></param>
    /// <returns>(SaveFileData) SaveFile</returns>
    public SaveFileData LoadFromFile(SaveFileData SaveFile)
    {
        if (SaveFile == null)
        {
            Debug.LogWarning("Provided 'SaveFile' is null, cannot Load from File!", this);
            return null;
        }

        return LoadSave(SaveFile);
    }

    /// <summary>
    /// Load a from the Selected Save File Instance
    /// </summary>
    /// <returns>(SaveFileData) LoadedData</returns>
    public SaveFileData LoadFromFile()
    {
        if (SelectedSaveFile == null)
        {
            Debug.LogWarning("'SelectedSaveFile' is equal to null, cannot Load from null File!", this);
            return null;
        }

        return LoadSave(SelectedSaveFile);
    }

    /// <summary>
    /// Save Data using a SaveFileData Instance to disk.
    /// </summary>
    /// <param name="SaveFile"></param>
    private void SaveData(SaveFileData SaveFile)
    {
        BinaryFormatter formater = new();
        FileStream saveFile = File.Create(DirectoryPath + "/" + SaveFile.FileName + ".bin");

        formater.Serialize(saveFile, SaveFile);
        saveFile.Close();

        Debug.LogWarning("Successfully Saved: " + SaveFile.FileName + GlobalFileType + " To: " + DirectoryPath, this);
    }

    /// <summary>
    /// Save Data using a SaveFileData Instance to disk.
    /// </summary>
    /// <param name="SaveFile"></param>
    public void SaveToFile(SaveFileData SaveFile)
    {
        if (SaveFile == null)
        {
            Debug.LogWarning("Provided 'SaveFile' is null, cannot Save to File!", this);
            return;
        }

        SaveData(SaveFile);
    }

    /// <summary>
    /// Save the Selected Save File's data to disk.
    /// </summary>
    public void SaveToFile()
    {
        if (SelectedSaveFile == null)
        {
            Debug.LogWarning("Provided, 'SaveFile' is null, cannot Save to a Null File!", this);
            return;
        }

        SaveData(SelectedSaveFile);
    }

    /// <summary>
    /// Assign Instance to SaveFileManager.cs, create a directory using the DirectoryPath variable if it doesn't exist and initalize all save files.
    /// </summary>
    private void Start()
    {
        Instance ??= this;
        
        if (DirectoryPath == string.Empty)
            DirectoryPath = Application.persistentDataPath + "/" + DirectoryName;
        else
            DirectoryPath = DirectoryPath + "/" + DirectoryName;

        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);

        for (int i = 0; i < SaveFiles.Length; i++)
        {
            if (!File.Exists(DirectoryPath + "/" + SaveFiles[i].FileName + GlobalFileType))
                SaveToFile(SaveFiles[i]);
            else
                SaveFiles[i] = LoadFromFile(SaveFiles[i]);
        }
    }
}
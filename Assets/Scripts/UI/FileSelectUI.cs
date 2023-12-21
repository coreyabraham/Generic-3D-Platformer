using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Handler for File Management
/// [ Uses: SaveFileManager.cs, PromptUI.cs ]
/// </summary>
public class FileSelectUI : MonoBehaviour
{
    /// <summary>
    /// Event Arguments used for Prompting.
    /// </summary>
    [Serializable]
    public class EventArgs
    {
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public string Body { get; set; }
        public Action<bool> Action { get; set; }
    }

    [field: Header("Save Files")]
    [field: SerializeField] public Button SaveFile1 { get; set; }
    [field: SerializeField] public Button SaveFile2 { get; set; }
    [field: SerializeField] public Button SaveFile3 { get; set; }

    [field: Header("Assets")]
    [field: SerializeField] public TMP_Text OutputLabel { get; set; }
    [field: SerializeField] public Button LoadBtn { get; set; }
    [field: SerializeField] public Button ResetBtn { get; set; }
    [field: SerializeField] public Button ExitBtn { get; set; }

    private int SelectedFile;
    private EventArgs ResetArgs;

    /// <summary>
    /// Temp Clicked, Set Selected Save File to default then load scene.
    /// </summary>
    /// <param name="SceneName"></param>
    public void TempClicked(string SceneName)
    {
        if (SaveFileManager.Instance.SelectedSaveFile == null)
            SaveFileManager.Instance.SetSelectedSaveFile();

        SceneController.Instance.LoadScene(SceneName);
    }

    /// <summary>
    /// Check SaveFile on disk validity.
    /// </summary>
    /// <returns>(bool) status</returns>
    private bool ValidateSelection()
    {
        string message = "Selected: " + SaveFileManager.Instance.SaveFiles[SelectedFile - 1].FriendlyName + "!";
        bool status = true;

        if (SelectedFile == 0)
        {
            message = "You cannot Load / Delete a File if you haven't selected one, try clicking on one first!";
            status = false;
        }
        
        else
            SaveFileManager.Instance.SetSelectedSaveFile(SaveFileManager.Instance.SaveFiles[SelectedFile - 1]);

        OutputLabel.text = message;
        return status;
    }

    /// <summary>
    /// Load the Selected Save File.
    /// </summary>
    private void LoadSelected()
    {
        bool result = ValidateSelection();
        
        if (!result)
            return;

        SaveFileData data = SaveFileManager.Instance.LoadFromFile();

        int index = 0;
        for (int i = 0; i < SceneController.Instance.LinearLevels.Length; i++)
        {
            if (SceneController.Instance.LinearLevels[i] == data.LevelName)
            {
                index = i;
                break;
            }
        }

        SceneController.Instance.LoadScene(data.LevelName, index);
    }

    /// <summary>
    /// Delete the Selected Save File.
    /// </summary>
    private void DeleteSelected()
    {
        bool result = ValidateSelection();

        if (!result)
            return;

        SaveFileData data = SaveFileManager.Instance.LoadFromFile();

        void Execute(bool result)
        {
            if (!result)
                return;

            SaveFileData newData = new()
            {
                FileName = data.FileName,
                FriendlyName = data.FriendlyName,
                LevelName = SaveFileManager.Instance.DefaultSaveFileData.LevelName,
                GoldCount = SaveFileManager.Instance.DefaultSaveFileData.GoldCount,
                LivesCount = SaveFileManager.Instance.DefaultSaveFileData.LivesCount
            };

            SaveFileManager.Instance.SaveToFile(newData);
        }

        ResetArgs.Action = Execute;
        PromptUI.Instance.StartPrompt(ResetArgs.Title, ResetArgs.Body, ResetArgs.Action);
    }

    /// <summary>
    /// Hide this UI when exit clicked.
    /// </summary>
    private void ExitSelected()
    {
        if (!gameObject.activeSelf)
            return;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Set SelectedFile integer to 1 and validate data.
    /// </summary>
    private void Save1Selected() { SelectedFile = 1; ValidateSelection(); }

    /// <summary>
    /// Set SelectedFile integer to 2 and validate data.
    /// </summary>
    private void Save2Selected() { SelectedFile = 2; ValidateSelection(); }

    /// <summary>
    /// Set SelectedFile integer to 3 and validate data.
    /// </summary>
    private void Save3Selected() { SelectedFile = 3; ValidateSelection(); }

    /// <summary>
    /// Format the Output Text Label depending on file status.
    /// </summary>
    /// <param name="TextLabel"></param>
    /// <param name="SaveData"></param>
    private void FormatText(TMP_Text TextLabel, SaveFileData SaveData) => TextLabel.text = SaveData.FriendlyName + " // " + SaveData.LevelName;

    /// <summary>
    /// Create reset args prompt title and body, adjust all save text labels and hook listener events to methods.
    /// </summary>
    private void Start()
    {
        ResetArgs = new()
        {
            Title = "< Delete File >",
            Body = "Are you sure you want to reset this file?"
        };

        TMP_Text text = SaveFile1.GetComponentInChildren<TMP_Text>();
        FormatText(text, SaveFileManager.Instance.SaveFiles[0]);

        text = SaveFile2.GetComponentInChildren<TMP_Text>();
        FormatText(text, SaveFileManager.Instance.SaveFiles[1]);

        text = SaveFile3.GetComponentInChildren<TMP_Text>();
        FormatText(text, SaveFileManager.Instance.SaveFiles[2]);

        SaveFile1.onClick.AddListener(Save1Selected);
        SaveFile2.onClick.AddListener(Save2Selected);
        SaveFile3.onClick.AddListener(Save3Selected);

        LoadBtn.onClick.AddListener(LoadSelected);
        ResetBtn.onClick.AddListener(DeleteSelected);
        ExitBtn.onClick.AddListener(ExitSelected);
    }
}

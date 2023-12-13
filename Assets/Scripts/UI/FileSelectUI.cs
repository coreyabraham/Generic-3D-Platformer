using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileSelectUI : MonoBehaviour
{
    [Serializable]
    public class EventArgs
    {
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public string Body { get; set; }
        [field: SerializeField] public Action<bool> Action { get; set; }
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

    public void TempClicked(string SceneName)
    {
        if (SaveFileManager.Instance.SelectedSaveFile == null)
            SaveFileManager.Instance.SetSelectedSaveFile();

        SceneController.Instance.LoadScene(SceneName);
    }

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

    private void LoadSelected()
    {
        bool result = ValidateSelection();
        
        if (!result)
            return;

        // This doesn't run due to the scene changing and this code not running, please fix!
        void Test() => print("UPDATE GAME / PLAYER VALUES HERE!");

        SaveFileData data = SaveFileManager.Instance.LoadFromFile();

        Debug.Log("DATA FINISHED LOADING: " + data.FriendlyName, this);
        SceneController.Instance.LoadScene(data.LevelName, Test);
    }

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

    private void ExitSelected()
    {
        if (!gameObject.activeSelf)
            return;

        gameObject.SetActive(false);
    }

    private void Save1Selected() { SelectedFile = 1; ValidateSelection(); }
    private void Save2Selected() { SelectedFile = 2; ValidateSelection(); }
    private void Save3Selected() { SelectedFile = 3; ValidateSelection(); }

    private void Start()
    {
        ResetArgs = new()
        {
            Title = "< Delete File >",
            Body = "Are you sure you want to reset this file?"
        };

        TMP_Text text = SaveFile1.GetComponentInChildren<TMP_Text>();
        text.text = SaveFileManager.Instance.SaveFiles[0].FriendlyName;

        text = SaveFile2.GetComponentInChildren<TMP_Text>();
        text.text = SaveFileManager.Instance.SaveFiles[1].FriendlyName;
        
        text = SaveFile3.GetComponentInChildren<TMP_Text>();
        text.text = SaveFileManager.Instance.SaveFiles[2].FriendlyName;

        SaveFile1.onClick.AddListener(Save1Selected);
        SaveFile2.onClick.AddListener(Save2Selected);
        SaveFile3.onClick.AddListener(Save3Selected);

        LoadBtn.onClick.AddListener(LoadSelected);
        ResetBtn.onClick.AddListener(DeleteSelected);
        ExitBtn.onClick.AddListener(ExitSelected);
    }
}

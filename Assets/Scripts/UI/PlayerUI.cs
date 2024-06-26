using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Handle Player HUD / Interface.
/// </summary>
public class PlayerUI : MonoBehaviour
{
    /// <summary>
    /// The type of Label to change.
    /// </summary>
    private enum LabelType
    {
        Life = 0,
        Gold
    }

    [field: Header("Frames")]
    [field: SerializeField] private GameObject Main { get; set; }
    [field: SerializeField] private GameObject Mobile { get; set; }
    [field: SerializeField] private GameObject Complete { get; set; }

    [field: Header("Miscellaneous")]
    [field: SerializeField] private TMP_Text LifeLabel { get; set; }
    [field: SerializeField] private TMP_Text GoldLabel { get; set; }
    [field: SerializeField] private TMP_Text FinishSublabel { get; set; }

    [field: Header("Settings")]
    [field: SerializeField] private string FinishedSublabelText { get; set; }
    [field: SerializeField] private int IntervalSeconds { get; set; }
    [field: SerializeField] private bool EmulateMobile { get; set; }
    [field: SerializeField] private string LevelCompleteSound { get; set; }

    private bool _levelCompleted;

    public void ToggleUI(bool result) => Main.SetActive(result);
    public void LevelFinished(string value) => LevelCompleteUI(value);
    public void LivesChanged(int value) => UpdateText(LabelType.Life, value);
    public void GoldChanged(int value) => UpdateText(LabelType.Gold, value);

    /// <summary>
    /// On Level Completion, pause game logic, run timer and move to next scene.
    /// </summary>
    /// <param name="SceneOverride"></param>
    private async void LevelCompleteUI(string SceneOverride)
    {
        if (_levelCompleted)
            return;

        _levelCompleted = true;
        Time.timeScale = 0.0f;
        
        ToggleUI(false);
        Complete.SetActive(true);
        AudioManager.Instance.Play(LevelCompleteSound);

        for (int i = IntervalSeconds; i >= 0; i--)
        {
            FinishSublabel.text = FinishedSublabelText.Replace("{SECONDS}", i.ToString());
            await Task.Delay(IntervalSeconds * 500);
        }

        _levelCompleted = false;
        Complete.SetActive(false);
        Time.timeScale = 1.0f;

        Debug.Log("SceneOverride: " + SceneOverride);

        if (SceneOverride == string.Empty)
            SceneController.Instance.LoadFromIndex();
        else
            SceneController.Instance.LoadScene(SceneOverride);

        SaveFileManager.Instance.SaveToFile();
    }

    /// <summary>
    /// Update Labels depending on the LabelType enum.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    private void UpdateText(LabelType type, int value)
    {
        switch (type)
        {
            case LabelType.Life: LifeLabel.text = "Lives: " + value.ToString(); break;
            case LabelType.Gold: GoldLabel.text = "Gold: " + value.ToString(); break;
        }
    }

    /// <summary>
    /// Turn off UI if enabled on start + enable Mobile UI if running on Mobile.
    /// </summary>
    private void Start()
    {
        if (Main.activeSelf)
            ToggleUI(false);

        if (EmulateMobile|| GameManager.Instance.PlatformType == PlatformTypes.Mobile)
            Mobile.SetActive(true);
    }
}

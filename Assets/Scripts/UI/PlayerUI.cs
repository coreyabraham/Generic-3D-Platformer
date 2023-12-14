using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private enum LabelType
    {
        Life = 0,
        Gold
    }

    [System.Serializable]
    private struct LevelFinishText
    {
        [field: SerializeField] private PlatformTypes PlatformType { get; set; }
        [field: SerializeField] private string Text { get; set; }
    }

    [field: Header("Frames")]
    [field: SerializeField] private GameObject Main { get; set; }
    [field: SerializeField] private GameObject Complete { get; set; }

    [field: Header("Miscellaneous")]
    [field: SerializeField] private TMP_Text LifeLabel { get; set; }
    [field: SerializeField] private TMP_Text GoldLabel { get; set; }
    [field: SerializeField] private LevelFinishText[] FinishingTexts { get; set; }

    private bool _levelCompleted;
    private bool _checkForInput;

    private float _buffer;
    private float _maxBuffer = 1.0f;

    public void ToggleUI(bool result) => Main.SetActive(result);
    public void LevelFinished() => LevelCompleteUI();
    public void LivesChanged(int value) => UpdateText(LabelType.Life, value);
    public void GoldChanged(int value) => UpdateText(LabelType.Gold, value);

    private void LevelCompleteUI()
    {
        if (_levelCompleted)
            return;

        _levelCompleted = true;
        Time.timeScale = 0.0f;
        
        ToggleUI(false);
        Complete.SetActive(true);


    }

    private void UpdateText(LabelType type, int value)
    {
        switch (type)
        {
            case LabelType.Life: LifeLabel.text = "Lives: " + value.ToString(); break;
            case LabelType.Gold: GoldLabel.text = "Gold: " + value.ToString(); break;
        }
    }

    private void Update()
    {
        if (_levelCompleted)
        {
            if (_buffer < _maxBuffer)
            {
                _buffer += _maxBuffer * Time.deltaTime;
                return;
            }
        }
    }

    private void Start()
    {
        if (Main.activeSelf)
            ToggleUI(false);
    }
}

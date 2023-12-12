using System.Collections.Generic;
using System.Threading.Tasks;

using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; internal set; }
    [field: SerializeField] public TMP_Text TextLabel { get; set; }
    [field: SerializeField] public List<GoldInstance> Golds { get; set; }

    private void UpdateText()
    {
        if (!TextLabel.enabled)
            TextLabel.enabled = true;

        TextLabel.text = "Gold: " + SaveFileManager.Instance.SelectedSaveFile.GoldCount.ToString();
    }

    private int GetTypeValue(GoldTypes gold)
    {
        switch (gold)
        {
            case GoldTypes.Regular: return 1;
            case GoldTypes.Big: return 5;
            case GoldTypes.Mega: return 10;
        }

        return 0;
    }

    public void ModifyGold(GoldInstance gold, bool increase = true)
    {
        int value = GetTypeValue(gold.GoldType);
        string soundName = string.Empty;

        switch (gold.GoldType)
        {
            case GoldTypes.Regular: soundName = "Regular"; break;
            case GoldTypes.Big: soundName = "Big"; break;
            case GoldTypes.Mega: soundName = "Mega"; break;
        }

        if (!increase)
        {
            soundName = "Lost";
            value = -value;
        }

        AudioManager.Instance.PlaySFX(soundName + " Gold");

        SaveFileManager.Instance.SelectedSaveFile.GoldCount += value;
        TextLabel.text = "Gold: " + SaveFileManager.Instance.SelectedSaveFile.GoldCount.ToString();

        if (SaveFileManager.Instance.SelectedSaveFile.GoldCount < 1)
            SaveFileManager.Instance.SelectedSaveFile.GoldCount = 0;

        Golds.Add(gold);
        UpdateText();
    }

    public async void ClearGold(int soundInterval = 0)
    {
        Task LocalPlaySound()
        {
            AudioManager.Instance.Play("Lost Gold");
            return Task.CompletedTask;
        }

        foreach (GoldInstance gold in Golds)
        {
            SaveFileManager.Instance.SelectedSaveFile.GoldCount -= GetTypeValue(gold.GoldType);
            UpdateText();

            if (soundInterval != 0)
            {
                Task _ = LocalPlaySound();
                await Task.Delay(soundInterval * 1000);
            }
        }

        Golds.Clear();

        if (SaveFileManager.Instance.SelectedSaveFile.GoldCount != 0)
        {
            SaveFileManager.Instance.SelectedSaveFile.GoldCount = 0;
            UpdateText();
        }
    }

    private void Start()
    {
        if (TextLabel == null)
            return;

        if (!TextLabel.enabled)
            TextLabel.enabled = true;

        int value = 0;
        bool enabled = SaveFileManager.Instance.SelectedSaveFile != null;

        if (enabled)
            value = SaveFileManager.Instance.SelectedSaveFile.GoldCount;

        TextLabel.enabled = enabled;
        TextLabel.text = "Gold: " + value.ToString();
    }

    private void Awake() => Instance ??= this;
}

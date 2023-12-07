using System.Collections.Generic;
using System.Threading.Tasks;

using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; internal set; }
    [field: SerializeField] public TMP_Text TextLabel { get; set; }
    [field: SerializeField] public List<GoldInstance> Golds { get; set; }

    private void UpdateText() => TextLabel.text = "Gold: " + GameManager.Instance.CurrentSaveFile.GoldCount.ToString();

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
        int value = 0;
        string soundName = string.Empty;

        switch (gold.GoldType) // convert this to "GetTypeValue()"!
        {
            case GoldTypes.Regular: value = 1; soundName = "Regular"; break;
            case GoldTypes.Big: value = 5; soundName = "Big"; break;
            case GoldTypes.Mega: value = 10; soundName = "Mega"; break;
        }

        if (!increase)
        {
            soundName = "Lost";
            value = -value;
        }

        AudioManager.Instance.PlaySFX(soundName + " Gold");

        GameManager.Instance.CurrentSaveFile.GoldCount += value;
        TextLabel.text = "Gold: " + GameManager.Instance.CurrentSaveFile.GoldCount.ToString();

        if (GameManager.Instance.CurrentSaveFile.GoldCount < 1)
            GameManager.Instance.CurrentSaveFile.GoldCount = 0;

        Golds.Add(gold);
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
            GameManager.Instance.CurrentSaveFile.GoldCount -= GetTypeValue(gold.GoldType);
            UpdateText();

            if (soundInterval != 0)
            {
                Task _ = LocalPlaySound();
                await Task.Delay(soundInterval * 1000);
            }
        }

        Golds.Clear();

        if (GameManager.Instance.CurrentSaveFile.GoldCount != 0)
        {
            GameManager.Instance.CurrentSaveFile.GoldCount = 0;
            UpdateText();
        }
    }

    private void Start() => TextLabel.text = "Gold: " + GameManager.Instance.CurrentSaveFile.GoldCount.ToString();
    private void Awake() => Instance ??= this;
}

using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; internal set; }
    [field: SerializeField] public TMP_Text TextLabel { get; set; }

    public void ModifyGold(GoldTypes goldType, bool increase = true)
    {
        int value = 0;
        string soundName = string.Empty;

        switch (goldType)
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

        if (GameManager.Instance.CurrentSaveFile.GoldCount < 0)
            GameManager.Instance.CurrentSaveFile.GoldCount = 0;
    }

    private void Start() => TextLabel.text = "Gold: " + GameManager.Instance.CurrentSaveFile.GoldCount.ToString();
    private void Awake() => Instance ??= this;
}

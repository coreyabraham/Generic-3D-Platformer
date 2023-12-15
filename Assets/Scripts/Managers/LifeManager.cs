using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance { get; internal set; }
    [field: SerializeField] public List<LifeInstance> Lives { get; set; }

    private int GetTypeValue(LifeTypes life)
    {
        switch (life)
        {
            case LifeTypes.Regular: return 1;
            case LifeTypes.Big: return 2;
            case LifeTypes.Mega: return 3;
        }

        return 0;
    }

    public void ModifyLives(bool increment, LifeInstance life = null)
    {
        int value = GetTypeValue(life.LifeType);
        string soundName = string.Empty;

        switch (life.LifeType)
        {
            case LifeTypes.Regular: soundName = "Regular"; break;
            case LifeTypes.Big: soundName = "Big"; break;
            case LifeTypes.Mega: soundName = "Mega"; break;
        }

        if (!increment)
        {
            soundName = "Lost";
            value = -value;
        }

        AudioManager.Instance.PlaySFX(soundName + " Gold");

        SaveFileManager.Instance.SelectedSaveFile.LivesCount += value;

        if (SaveFileManager.Instance.SelectedSaveFile.LivesCount < 1)
            SaveFileManager.Instance.SelectedSaveFile.LivesCount = 0;

        Lives.Add(life);
    }

    public void ModifyLives(bool increment, LifeTypes life = LifeTypes.Regular)
    {
        int value = GetTypeValue(life);
        string soundName = string.Empty;

        switch (life)
        {
            case LifeTypes.Regular: soundName = "Regular"; break;
            case LifeTypes.Big: soundName = "Big"; break;
            case LifeTypes.Mega: soundName = "Mega"; break;
        }

        if (!increment)
        {
            soundName = "Lost";
            value = -value;
        }

        AudioManager.Instance.PlaySFX(soundName + " Gold");

        SaveFileManager.Instance.SelectedSaveFile.LivesCount += value;

        if (SaveFileManager.Instance.SelectedSaveFile.LivesCount < 1)
        {
            SceneController.Instance.LoadScene("TitleScreen");
            //SaveFileManager.Instance.SelectedSaveFile.LivesCount = SaveFileManager.Instance.DefaultSaveFileData.LivesCount;
        }
    }

    private void Awake() => Instance ??= this;
}

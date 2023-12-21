using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle all Gold Interactions.
/// [ Uses: LifeInstance.cs, SaveFileManager.cs, AudioManager.cs ]
/// </summary>
public class LifeManager : MonoBehaviour
{
    /// <summary>
    /// Life Manager : Instance / Singleton
    /// </summary>
    public static LifeManager Instance { get; internal set; }
    private List<LifeInstance> Lives = new();

    /// <summary>
    /// Return an integer depending on a LifeTypes enum worth.
    /// </summary>
    /// <param name="life"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Handles Increasing / Decreasing, Sound Playback and "SelectedSaveFile" Modifications.
    /// </summary>
    /// <param name="increment"></param>
    /// <param name="life"></param>
    public void ModifyLives(bool increment, LifeInstance life = null)
    {
        // Get int value and initalize sound name for later use.
        int value = GetTypeValue(life.LifeType);
        string soundName = string.Empty;

        // Cycle through LifeTypes and determine what sound name it'll use.
        switch (life.LifeType)
        {
            case LifeTypes.Regular: soundName = "Regular"; break;
            case LifeTypes.Big: soundName = "Big"; break;
            case LifeTypes.Mega: soundName = "Mega"; break;
        }

        // Assign the sound name to "Lost" and add a negative modifier to value if increase was left as false 
        if (!increment)
        {
            soundName = "Lost";
            value = -value;
        }

        // Play the sound and modify the save file data.
        AudioManager.Instance.PlaySFX(soundName + " Gold");

        SaveFileManager.Instance.SelectedSaveFile.LivesCount += value;

        if (SaveFileManager.Instance.SelectedSaveFile.LivesCount < 1)
            SaveFileManager.Instance.SelectedSaveFile.LivesCount = 0;

        // Add the Life Instance into the list.
        Lives.Add(life);
    }

    /// <summary>
    /// Clears all Life Instances found withihn the "Lives" List.
    /// </summary>
    /// <param name="increment"></param>
    /// <param name="life"></param>
    public void ModifyLives(bool increment, LifeTypes life = LifeTypes.Regular)
    {
        // Play the sound locally on a different thread. 
        int value = GetTypeValue(life);
        string soundName = string.Empty;

        // Cycle through LifeTypes and determine what sound name it'll use.
        switch (life)
        {
            case LifeTypes.Regular: soundName = "Regular"; break;
            case LifeTypes.Big: soundName = "Big"; break;
            case LifeTypes.Mega: soundName = "Mega"; break;
        }

        // Assign the sound name to "Lost" and add a negative modifier to value if increase was left to false.
        if (!increment)
        {
            soundName = "Lost";
            value = -value;
        }

        // Play the sound and mofidy the save file data.
        AudioManager.Instance.PlaySFX(soundName + " Gold");

        SaveFileManager.Instance.SelectedSaveFile.LivesCount += value;

        // If LivesCount is equal to zero or below, then return to the Title Screen.
        if (SaveFileManager.Instance.SelectedSaveFile.LivesCount <= 0)
        {
            SceneController.Instance.LoadScene("TitleScreen");
            //SaveFileManager.Instance.SelectedSaveFile.LivesCount = SaveFileManager.Instance.DefaultSaveFileData.LivesCount;
        }
    }

    /// <summary>
    /// Assign Instance to LifeManager.cs if it was equal to null.
    /// </summary>
    private void Awake() => Instance ??= this;
}

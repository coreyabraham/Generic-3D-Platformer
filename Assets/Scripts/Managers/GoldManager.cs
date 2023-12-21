using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Manage all Gold Interactions.
/// [ Uses: GoldInstance.cs, SaveFileManager.cs, AudioManager.cs ]
/// </summary>
public class GoldManager : MonoBehaviour
{
    /// <summary>
    /// Gold Manager : Instance / Singleton
    /// </summary>
    public static GoldManager Instance { get; internal set; }
    private List<GoldInstance> Golds = new();

    /// <summary>
    /// Return an integer depending on a GoldTypes enum worth.
    /// </summary>
    /// <param name="gold"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Handles Increasing / Decreasing, Sound Playback and "SelectedSaveFile" Modifications.
    /// </summary>
    /// <param name="gold"></param>
    /// <param name="increase"></param>
    public void ModifyGold(GoldInstance gold, bool increase = true)
    {
        // Get int value and initalize sound name for later use.
        int value = GetTypeValue(gold.GoldType);
        string soundName = string.Empty;

        // Cycle through GoldTypes and determine what sound name it'll use.
        switch (gold.GoldType)
        {
            case GoldTypes.Regular: soundName = "Regular"; break;
            case GoldTypes.Big: soundName = "Big"; break;
            case GoldTypes.Mega: soundName = "Mega"; break;
        }

        // Assign the sound name to "Lost" and add a negative modifier to value if increase was left as false.
        if (!increase)
        {
            soundName = "Lost";
            value = -value;
        }

        // Play the sound and modify the save file data.
        AudioManager.Instance.PlaySFX(soundName + " Gold");

        SaveFileManager.Instance.SelectedSaveFile.GoldCount += value;

        if (SaveFileManager.Instance.SelectedSaveFile.GoldCount < 1)
            SaveFileManager.Instance.SelectedSaveFile.GoldCount = 0;

        // Add the Gold Instance into the list.
        Golds.Add(gold);
    }

    /// <summary>
    /// Clears all Gold Instances found within the "Golds" List.
    /// </summary>
    /// <param name="soundInterval"></param>
    public async void ClearGold(int soundInterval = 0)
    {
        // Play the sound locally on a different thread.
        Task LocalPlaySound()
        {
            AudioManager.Instance.Play("Lost Gold");
            return Task.CompletedTask;
        }

        // Run through each Gold Instance, decrease the "SelectedSaveFile.GoldCount" value by the Gold's "GoldType" and play a deduction sound with it.
        foreach (GoldInstance gold in Golds)
        {
            SaveFileManager.Instance.SelectedSaveFile.GoldCount -= GetTypeValue(gold.GoldType);

            if (soundInterval != 0)
            {
                Task _ = LocalPlaySound();
                await Task.Delay(soundInterval * 1000);
            }
        }

        // Clear all Gold Instances from the List and revert the GoldCount back to zero.
        Golds.Clear();

        if (SaveFileManager.Instance.SelectedSaveFile.GoldCount != 0)
            SaveFileManager.Instance.SelectedSaveFile.GoldCount = 0;
    }

    /// <summary>
    /// Assign Instance to GoldManager.cs if it was equal to null.
    /// </summary>
    private void Awake() => Instance ??= this;
}

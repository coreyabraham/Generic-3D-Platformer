using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handle Player Data (Gold and Lives)
/// [ Uses: SaveFileManager.cs ]
/// </summary>
public class HandlePlayerData : MonoBehaviour
{
    [field: SerializeField] private UnityEvent<int> GoldChanged { get; set; }
    [field: SerializeField] private UnityEvent<int> LivesChanged { get; set; }

    private int previousGoldCount = 0;
    private int previousLivesCount = 0;

    /// <summary>
    /// Initalizes Events
    /// </summary>
    private void Start()
    {
        GoldChanged ??= new();
        LivesChanged ??= new();
    }

    /// <summary>
    /// Check "SelectedSaveFile" (Within SaveFileManager.cs) null state + previous[Variable]Counts
    /// [ Uses: SaveFileManager.cs ]
    /// </summary>
    private void Update()
    {
        if (SaveFileManager.Instance.SelectedSaveFile == null)
            return;

        if (previousGoldCount != SaveFileManager.Instance.SelectedSaveFile.GoldCount)
        {
            previousGoldCount = SaveFileManager.Instance.SelectedSaveFile.GoldCount;
            GoldChanged?.Invoke(previousGoldCount);
        }

        if (previousLivesCount != SaveFileManager.Instance.SelectedSaveFile.LivesCount)
        {
            previousLivesCount = SaveFileManager.Instance.SelectedSaveFile.LivesCount;
            LivesChanged?.Invoke(previousLivesCount);
        }
    }
}

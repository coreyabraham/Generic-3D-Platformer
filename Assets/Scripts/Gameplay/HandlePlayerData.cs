using UnityEngine;
using UnityEngine.Events;

public class HandlePlayerData : MonoBehaviour
{
    [field: SerializeField] private UnityEvent<int> GoldChanged { get; set; }
    [field: SerializeField] private UnityEvent<int> LivesChanged { get; set; }

    private int previousGoldCount = 0;
    private int previousLivesCount = 0;

    private void Start()
    {
        GoldChanged ??= new();
        LivesChanged ??= new();
    }

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

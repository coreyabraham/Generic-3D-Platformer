using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; internal set; }

    [field: SerializeField] public string[] LinearLevels { get; set; }

    [field: SerializeField] private int _currentIndex = -1;
    private Action _carriedAction;

    public void SetCurrentIndex(int index)
    {
        #if !UNITY_EDITOR
            return;
        #endif

        _currentIndex = index;
    }

    public void LoadFromIndex(bool loadBackwards = false, Action action = null)
    {
        int modifier = !loadBackwards ? 1 : -1;

        if (LinearLevels[_currentIndex] + modifier == null)
        {
            Debug.LogWarning("Error loading target scene with index: " + _currentIndex.ToString() + ", it may not exist within 'LinearLevels[" + LinearLevels.Length.ToString() + "]!", this);
            return;
        }

        _currentIndex += modifier;
        LoadScene(LinearLevels[_currentIndex], 0, action);
    }

    public void LoadScene(string targetScene, int modifyIndex = 0, Action action = null)
    {
        if (modifyIndex != 0)
        {
            if (LinearLevels[modifyIndex] == null)
                return;

            _currentIndex = modifyIndex;
        }

        if (SaveFileManager.Instance.SelectedSaveFile != null)
            SaveFileManager.Instance.SelectedSaveFile.LevelName = targetScene;

        SceneManager.LoadScene(targetScene);
        SceneManager.sceneLoaded += SceneLoaded;

        if (_carriedAction != null)
            _carriedAction = action;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _carriedAction?.Invoke();
        _carriedAction = null;
    }

    private void Awake() => Instance ??= this;
}

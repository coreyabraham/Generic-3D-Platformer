using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle Scene Management within Unity.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Scene Controller : Instance / Singleton
    /// </summary>
    public static SceneController Instance { get; internal set; }

    [field: SerializeField] public string[] LinearLevels { get; set; }

    [field: SerializeField] private int _currentIndex = -1;
    private Action _carriedAction;

    /// <summary>
    /// Sets the Current Index integer to the provided value.
    /// </summary>
    /// <param name="index"></param>
    public void SetCurrentIndex(int index)
    {
        #if !UNITY_EDITOR
            return;
        #endif

        _currentIndex = index;
    }

    /// <summary>
    /// Load the next Unity Scene from the Current Index integer within the "LinearLevels" array.
    /// </summary>
    /// <param name="loadBackwards"></param>
    /// <param name="action"></param>
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

    /// <summary>
    /// Load a specific scene using a target scene's name.
    /// </summary>
    /// <param name="targetScene"></param>
    /// <param name="modifyIndex"></param>
    /// <param name="action"></param>
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

    /// <summary>
    /// Run a stored Action after a scene is loaded.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _carriedAction?.Invoke();
        _carriedAction = null;
    }

    /// <summary>
    /// Assign the Instance to SceneController.cs if null.
    /// </summary>
    private void Awake() => Instance ??= this;
}

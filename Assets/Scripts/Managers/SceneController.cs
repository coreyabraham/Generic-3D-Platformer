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

    public void LoadFromIndex(bool loadBackwards = false, Action action = null)
    {
        int modifier = !loadBackwards ? 1 : -1;

        if (LinearLevels[_currentIndex] + modifier == null)
        {
            Debug.Log("Error loading target scene with index: " + _currentIndex.ToString() + ", it may not exist within 'LinearLevels[" + LinearLevels.Length.ToString() + "]!", this);
            return;
        }

        _currentIndex += modifier;
        LoadScene(LinearLevels[_currentIndex], action);
    }

    public void LoadScene(string targetScene, Action action = null)
    {
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

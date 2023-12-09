using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; internal set; }

    [field: SerializeField] public string LoadOnStartup { get; set; }

    private Action carriedAction;

    public void LoadScene(string targetScene, Action action = null)
    {
        SceneManager.LoadScene(targetScene);
        SceneManager.sceneLoaded += SceneLoaded;

        if (carriedAction != null)
            carriedAction = action;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        carriedAction?.Invoke();
        carriedAction = null;
    }

    private void Awake()
    {
        Instance ??= this;

        if (LoadOnStartup != string.Empty)
            SceneManager.LoadScene(LoadOnStartup);
    }
}

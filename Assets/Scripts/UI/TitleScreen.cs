using System;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base Title Screen Logic.
/// [ Uses: PromptUI.cs ]
/// </summary>
public class TitleScreen : MonoBehaviour
{
    /// <summary>
    /// Event Arguments used for Prompting.
    /// </summary>
    [Serializable]
    public class EventArgs
    {
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public string Body { get; set; }
        public Action<bool> Action { get; set; }
    }

    [field: Header("Buttons")]
    [field: SerializeField] public Button PlayBtn { get; set; }
    [field: SerializeField] public Button SettingsBtn { get; set; }
    [field: SerializeField] public Button ExitBtn { get; set; }

    [field: Header("Menus")]
    [field: SerializeField] public GameObject PlayMenu { get; set; }
    [field: SerializeField] public GameObject SettingsMenu { get; set; }

    [field: Header("Miscellaneous")]
    [field: SerializeField] public EventArgs ExitArgs { get; set; }

    private bool ButtonsEnabled = false;

    /// <summary>
    /// Show the Save Files menu when clicked.
    /// </summary>
    private void PlayClicked()
    {
        if (!ButtonsEnabled || PlayMenu.activeSelf)
            return;

        PlayMenu.SetActive(true);
    }

    /// <summary>
    /// Show the Settings menu when clicked.
    /// </summary>
    private void SettingsClicked()
    {
        if (!ButtonsEnabled || SettingsMenu.activeSelf)
            return;

        SettingsMenu.SetActive(true);
    }

    /// <summary>
    /// Prompt the PromptUI Instance when clicked. Quit the game when returned true.
    /// </summary>
    private void ExitClicked()
    {
        if (!ButtonsEnabled)
            return;

        void EventFunction(bool result)
        {
            ExitArgs.Action = null;

            if (!result)
                return;
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.Beep();
                UnityEditor.EditorApplication.isPlaying = false;
            #endif

            Application.Quit();
        }

        ExitArgs.Action = EventFunction;
        PromptUI.Instance.StartPrompt(ExitArgs.Title, ExitArgs.Body, ExitArgs.Action);
    }

    /// <summary>
    /// Hook "onClick" events to methods and fix Cursor State if broken.
    /// </summary>
    private void Start()
    {
        PlayBtn.onClick.AddListener(PlayClicked);
        SettingsBtn.onClick.AddListener(SettingsClicked);
        ExitBtn.onClick.AddListener(ExitClicked);

        ButtonsEnabled = true;

        if (Cursor.lockState != CursorLockMode.None)
            Cursor.lockState = CursorLockMode.None;

        if (!Cursor.visible)
            Cursor.visible = true;
    }
}

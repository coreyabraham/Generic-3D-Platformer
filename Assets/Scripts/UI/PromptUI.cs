using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle Prompt (Yes or No) Requests from all scripts.
/// </summary>
public class PromptUI : MonoBehaviour
{
    /// <summary>
    /// The Default Title, Body and Method that'll be used if no default was found.
    /// </summary>
    private class Defaults
    {
        public string Title;
        public string Body;
        public Action<bool> Action;
    }

    public static PromptUI Instance { get; internal set; }

    [field: Header("Assets")]
    [field: SerializeField] public GameObject Menu { get; set; }
    [field: SerializeField] public TMP_Text Title { get; set; }
    [field: SerializeField] public TMP_Text Body { get; set; }
    [field: SerializeField] public Button AcceptBtn { get; set; }
    [field: SerializeField] public Button DenyBtn { get; set; }

    private Defaults Assets = new();

    /// <summary>
    /// Invoke the provided method and run ResetPrompt().
    /// </summary>
    /// <param name="option"></param>
    public void FinishPrompt(bool option)
    {
        if (!Menu.activeSelf)
            return;

        Assets.Action?.Invoke(option);
        ResetPrompt();
    }

    /// <summary>
    /// Start a Prompt with a provided Title, Body and Action.
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Body"></param>
    /// <param name="Action"></param>
    public void StartPrompt(string Title, string Body, Action<bool> Action)
    {
        if (Menu.activeSelf)
            return;

        this.Title.text = Title;
        this.Body.text = Body;
        Assets.Action = Action;

        Menu.SetActive(true);
    }

    /// <summary>
    /// Reset the Prompt's properties before being used again.
    /// </summary>
    private void ResetPrompt()
    {
        if (!Menu.activeSelf)
            return;

        Title.text = Assets.Title;
        Body.text = Assets.Body;
        Assets.Action = null;

        Menu.SetActive(false);
    }

    /// <summary>
    /// Initalize Title and Body to their defaults + Add Hooks to Accept and Deny buttons.
    /// </summary>
    private void Start()
    {
        Assets.Title = Title.text;
        Assets.Body = Body.text;

        AcceptBtn.onClick.AddListener(delegate { FinishPrompt(true); });
        DenyBtn.onClick.AddListener(delegate { FinishPrompt(false); });
    }

    /// <summary>
    /// Hook Instance to PromptUI.cs if Instance is null.
    /// </summary>
    private void Awake() => Instance ??= this;
}

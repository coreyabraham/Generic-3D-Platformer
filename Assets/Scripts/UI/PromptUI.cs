using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PromptUI : MonoBehaviour
{
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

    public void FinishPrompt(bool option)
    {
        if (!Menu.activeSelf)
            return;

        Assets.Action?.Invoke(option);
        ResetPrompt();
    }

    public void StartPrompt(string Title, string Body, Action<bool> Action)
    {
        if (Menu.activeSelf)
            return;

        this.Title.text = Title;
        this.Body.text = Body;
        Assets.Action = Action;

        Menu.SetActive(true);
    }

    private void ResetPrompt()
    {
        if (!Menu.activeSelf)
            return;

        Title.text = Assets.Title;
        Body.text = Assets.Body;
        Assets.Action = null;

        Menu.SetActive(false);
    }

    private void Start()
    {
        Assets.Title = Title.text;
        Assets.Body = Body.text;

        AcceptBtn.onClick.AddListener(delegate { FinishPrompt(true); });
        DenyBtn.onClick.AddListener(delegate { FinishPrompt(false); });
    }

    private void Awake() => Instance ??= this;
}

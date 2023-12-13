using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private enum LabelType
    {
        Life = 0,
        Gold = 1
    }

    [field: SerializeField] private GameObject Main { get; set; }
    [field: SerializeField] private TMP_Text LifeLabel { get; set; }
    [field: SerializeField] private TMP_Text GoldLabel { get; set; }

    public void ToggleUI(bool result) => Main.SetActive(result);
    public void LivesChanged(int value) => UpdateText(LabelType.Life, value);
    public void GoldChanged(int value) => UpdateText(LabelType.Gold, value);

    private void UpdateText(LabelType type, int value)
    {
        switch (type)
        {
            case LabelType.Life: LifeLabel.text = "Lives: " + value.ToString(); break;
            case LabelType.Gold: GoldLabel.text = "Gold: " + value.ToString(); break;
        }
    }

    private void Start()
    {
        if (Main.activeSelf)
            ToggleUI(false);
    }
}

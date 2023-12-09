using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonSlider : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField] public bool AddPercentage { get; set; }
    [field: SerializeField] public int RoundingPoint { get; set; }

    private Slider slider;
    private TMP_InputField input;

    private void SetDisplay(float value)
    {
        double result = System.Math.Round(value, RoundingPoint);
        string display = AddPercentage ? result.ToString() + "%" : result.ToString();

        slider.value = value;
        input.text = display;
    }

    private void SliderChanged(float value) => SetDisplay(value);
    private void InputChanged(string value)
    {
        bool success = float.TryParse(value, out float result);
        
        if (!success)
            return;

        Debug.Log(value);
        Debug.Log(result);

        SetDisplay(result);
    }

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        input = GetComponentInChildren<TMP_InputField>();

        SetDisplay(slider.value);

        slider.onValueChanged.AddListener(SliderChanged);
        input.onEndEdit.AddListener(InputChanged);
    }
}

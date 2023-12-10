using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonSlider : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField] public bool AddPercentage { get; set; }
    [field: SerializeField] public int RoundingPoint { get; set; }

    [field: Header("Assets")]
    [field: SerializeField] public Slider Slider { get; set; }
    [field: SerializeField] public TMP_InputField Input { get; set; }

    private void SetDisplay(float value)
    {
        double result = System.Math.Round(value, RoundingPoint);
        string display = AddPercentage ? result.ToString() + "%" : result.ToString();

        Slider.value = value;
        Input.text = display;
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
        Slider = GetComponentInChildren<Slider>();
        Input = GetComponentInChildren<TMP_InputField>();

        SetDisplay(Slider.value);

        Slider.onValueChanged.AddListener(SliderChanged);
        Input.onEndEdit.AddListener(InputChanged);
    }
}

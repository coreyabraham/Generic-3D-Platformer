using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A Standardized Slider Instance used in the Settings Menu.
/// </summary>
public class CommonSlider : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField] public bool AddPercentage { get; set; }
    [field: SerializeField] public int RoundingPoint { get; set; }

    [field: Header("Assets")]
    [field: SerializeField] public Slider Slider { get; set; }
    [field: SerializeField] public TMP_InputField Input { get; set; }

    /// <summary>
    /// Update both Slider and Input text displays.
    /// </summary>
    /// <param name="value"></param>
    private void SetDisplay(float value)
    {
        double result = System.Math.Round(value, RoundingPoint);
        string display = AddPercentage ? result.ToString() + "%" : result.ToString();

        Slider.value = value;
        Input.text = display;
    }

    /// <summary>
    /// Invoke the SetDisplay() Action whenever the Slider is changed.
    /// </summary>
    /// <param name="value"></param>
    private void SliderChanged(float value) => SetDisplay(value);
    
    /// <summary>
    /// Invoke the SetDisplay() Action whenever the InputField is changed and the input was successfully converted to a float from string.
    /// </summary>
    /// <param name="value"></param>
    private void InputChanged(string value)
    {
        bool success = float.TryParse(value, out float result);
        
        if (!success)
            return;

        SetDisplay(result);
    }

    /// <summary>
    /// Initalize all Instances, Set the Display on startup and add Event Hooks.
    /// </summary>
    private void Start()
    {
        Slider = GetComponentInChildren<Slider>();
        Input = GetComponentInChildren<TMP_InputField>();

        SetDisplay(Slider.value);

        Slider.onValueChanged.AddListener(SliderChanged);
        Input.onEndEdit.AddListener(InputChanged);
    }
}

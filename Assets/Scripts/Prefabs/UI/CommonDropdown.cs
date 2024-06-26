using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A Standardized Dropdown Instance used in the Settings Menu.
/// </summary>
public class CommonDropdown : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField] public bool IgnoreListables { get; set; }
    [field: SerializeField] public bool CarryPreviousValues { get; set; }
    [field: SerializeField] public List<TMP_Dropdown.OptionData> Listables { get; set; }

    [field: Header("Assets")]
    [field: SerializeField] public TMP_Dropdown Dropdown { get; set; }
    
    private int previousValue;

    /// <summary>
    /// Apply all settings on startup as necessary.
    /// </summary>
    private void Start()
    {
        if (Dropdown == null)
            Dropdown = GetComponentInChildren<TMP_Dropdown>();
 
        if (CarryPreviousValues)
            previousValue = Dropdown.value;

        if (!IgnoreListables)
        {
            if (Dropdown.options.Count != 0)
                Dropdown.ClearOptions();

            Dropdown.AddOptions(Listables);
        }

        if (CarryPreviousValues)
        {
            Dropdown.value = previousValue;
            Dropdown.RefreshShownValue();
        }
    }
}

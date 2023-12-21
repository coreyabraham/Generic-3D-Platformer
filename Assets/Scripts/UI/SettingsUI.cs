using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle All Settings Menu Logic.
/// [ Uses: SettingsData.cs, AudioManager.cs, PromptUI.cs ]
/// </summary>
public class SettingsUI : MonoBehaviour
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

    [field: Header("Setup")]
    [field: SerializeField] public GameObject Main { get; set; }
    [field: SerializeField] public TMP_Text Title { get; set; }
    [field: SerializeField] public SettingsData Settings { get; set; }

    [field: Header("Audio and Video Navigation")]
    [field: SerializeField] public Button AudioBtn { get; set; }
    [field: SerializeField] public Button VideoBtn { get; set; }

    [field: SerializeField] public GameObject AudioFrame { get; set; }
    [field: SerializeField] public GameObject VideoFrame { get; set; }

    [field: Header("Other Buttons")]
    [field: SerializeField] public Button ApplyBtn { get; set; }
    [field: SerializeField] public Button RevertBtn { get; set; }
    [field: SerializeField] public Button ExitBtn { get; set; }

    [field: Header("Values > Audio")]
    [field: SerializeField] public CommonSlider MasterVolumeSlider { get; set; }
    [field: SerializeField] public CommonSlider SoundVolumeSlider { get; set; }
    [field: SerializeField] public CommonSlider MusicVolumeSlider { get; set; }

    [field: Header("Values > Video")]
    [field: SerializeField] public CommonDropdown ResolutionDropdown { get; set; }
    [field: SerializeField] public CommonDropdown QualityDropdown { get; set; }
    [field: SerializeField] public CommonSlider FPSSlider { get; set; }
    [field: SerializeField] public CommonSlider CamFOVSlider { get; set; }
    [field: SerializeField] public Toggle FullscreenToggle { get; set; }
    [field: SerializeField] public Toggle DisplayFPSToggle { get; set; }

    [field: Header("Arrays")]
    [field: SerializeField] public string[] MixerValues = new string[3];
    [field: SerializeField] public EventArgs[] EventArguments = new EventArgs[2];

    [HideInInspector] public BaseSettings CurrentSettings;

    /// <summary>
    /// Change between "Audio" and "Video" SubFrames.
    /// </summary>
    /// <param name="isAudio"></param>
    private void ChangeSubFrame(bool isAudio)
    {
        GameObject target1 = isAudio ? AudioFrame : VideoFrame;
        GameObject target2 = !isAudio ? AudioFrame : VideoFrame;

        target1.SetActive(true);
        target2.SetActive(false);

        Title.text = "< Settings - " + target1.name + " >";
    }

    /// <summary>
    /// Handle PromptUI when requesting settings applying.
    /// </summary>
    /// <param name="args"></param>
    private void ApplyRequested(EventArgs args)
    {
        void ActionRequest(bool result)
        {
            args.Action = null;

            if (!result)
                return;
            
            ApplySettings();
        }

        args.Action = ActionRequest;
        PromptUI.Instance.StartPrompt(args.Title, args.Body, args.Action);
    }

    /// <summary>
    /// Handle PromptUI when requesting settings resetting.
    /// </summary>
    /// <param name="args"></param>
    private void DenyRequested(EventArgs args)
    {
        void ActionRequest(bool result)
        {
            args.Action = null;

            if (!result)
                return;
            
            RevertSettings();
        }

        args.Action = ActionRequest;
        PromptUI.Instance.StartPrompt(args.Title, args.Body, args.Action);
    }

    /// <summary>
    /// Apply all "CurrentSettings" data to the user's SettingsData.json file.
    /// </summary>
    private void ApplySettings()
    {
        #if UNITY_EDITOR
            Debug.LogWarning("Playing in Unity Editor, some settings may not be visible!", this);
        #endif

        Resolution targetResolution = Settings.Resolutions[CurrentSettings.DisplayResolution];
        Screen.SetResolution(targetResolution.width, targetResolution.height, CurrentSettings.Fullscreen);

        QualitySettings.SetQualityLevel(CurrentSettings.GameQuality);
        Application.targetFrameRate = CurrentSettings.FPS;

        GameManager.Instance.FieldOfView = CurrentSettings.CamFOV;
        Settings.ApplySettings(CurrentSettings);
    }

    /// <summary>
    /// Revert all "CurrentSettings" and run the ApplySettings() method.
    /// </summary>
    private void RevertSettings()
    {
        Settings.SetDefaults();

        MasterVolumeSlider.Slider.value = CurrentSettings.MasterVolume;
        SoundVolumeSlider.Slider.value = CurrentSettings.SoundVolume;
        MusicVolumeSlider.Slider.value = CurrentSettings.MusicVolume;

        ResolutionDropdown.Dropdown.value = CurrentSettings.DisplayResolution;
        QualityDropdown.Dropdown.value = CurrentSettings.GameQuality;
        FullscreenToggle.isOn = CurrentSettings.Fullscreen;
        DisplayFPSToggle.isOn = CurrentSettings.DisplayFPS;
        FPSSlider.Slider.value = CurrentSettings.FPS;
        CamFOVSlider.Slider.value = CurrentSettings.CamFOV;

        ApplySettings();
    }

    /// <summary>
    /// Adjust Slider Value to match with AudioMixer decibel range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private float AudioSliderCalculations(float value) => Mathf.Log10(value) * 20;

    /// <summary>
    /// Adjust AudioMixer from "AudioManager.cs" based off of "AudioSliderCalculations(float value)"'s return value.
    /// </summary>
    /// <param name="valuePointer"></param>
    /// <param name="value"></param>
    private void AudioSliderChanged(int valuePointer, float value) => AudioManager.Instance.AudioMixer.SetFloat(MixerValues[valuePointer], AudioSliderCalculations(value));

    /// <summary>
    /// Master Volume Slider changed, invoke "AudioSliderChanged()".
    /// </summary>
    /// <param name="value"></param>
    private void MasterVolumeChanged(float value) { CurrentSettings.MasterVolume = value; AudioSliderChanged(0, value); }

    /// <summary>
    /// Sound Volume Slider changed, invoke "AudioSliderChanged()".
    /// </summary>
    /// <param name="value"></param>
    private void SoundVolumeChanged(float value) { CurrentSettings.SoundVolume = value; AudioSliderChanged(1, value); }

    /// <summary>
    /// Music Volume Slider changed, invoke "AudioSliderChanged()".
    /// </summary>
    /// <param name="value"></param>
    private void MusicVolumeChanged(float value) { CurrentSettings.MusicVolume = value; AudioSliderChanged(2, value); }

    /// <summary>
    /// FPS Slider changed, update CurrentSettings.FPS.
    /// </summary>
    /// <param name="value"></param>
    private void FPSChanged(float value) => CurrentSettings.FPS = (int)Mathf.Round(value);
    
    /// <summary>
    /// CamFOV changed, update CurrentSettings.CamFOV.
    /// </summary>
    /// <param name="value"></param>
    private void CamFOVChanged(float value) => CurrentSettings.CamFOV = (int)Mathf.Round(value);
    
    /// <summary>
    /// Fullscreen changed, update CurrentSettings.Fullscreen.
    /// </summary>
    /// <param name="value"></param>
    private void FullscreenChanged(bool value) => CurrentSettings.Fullscreen = value;
    
    /// <summary>
    /// Resolution changed, update CurrentSettings.DisplayResolution.
    /// </summary>
    /// <param name="value"></param>
    private void ResolutionChanged(int value) => CurrentSettings.DisplayResolution = value;
    
    /// <summary>
    /// Quality changed, update CurrentSettings.GameQuality and set QualityLevel via Unity facilities.
    /// </summary>
    /// <param name="value"></param>
    private void QualityChanged(int value) { CurrentSettings.GameQuality = value; QualitySettings.SetQualityLevel(value); }
    
    /// <summary>
    /// DisplayFPS Changed, update CurrentSettings.DisplayFPS.
    /// </summary>
    /// <param name="value"></param>
    private void DisplayFPSChanged(bool value) { CurrentSettings.DisplayFPS = value; }

    /// <summary>
    /// Refresh all values, settings and adjustments + Hook Events.
    /// </summary>
    private void PostInitSetup()
    {
        MasterVolumeSlider.Slider.value = CurrentSettings.MasterVolume;
        SoundVolumeSlider.Slider.value = CurrentSettings.SoundVolume;
        MusicVolumeSlider.Slider.value = CurrentSettings.MusicVolume;
        FPSSlider.Slider.value = CurrentSettings.FPS;
        CamFOVSlider.Slider.value = CurrentSettings.CamFOV;

        MasterVolumeSlider.Input.text = CurrentSettings.MasterVolume.ToString();
        SoundVolumeSlider.Input.text = CurrentSettings.SoundVolume.ToString();
        MusicVolumeSlider.Input.text = CurrentSettings.MusicVolume.ToString();
        FPSSlider.Input.text = CurrentSettings.FPS.ToString();
        CamFOVSlider.Input.text = CurrentSettings.CamFOV.ToString();

        QualityDropdown.Dropdown.value = CurrentSettings.GameQuality;
        QualitySettings.SetQualityLevel(CurrentSettings.GameQuality);

        FullscreenToggle.isOn = CurrentSettings.Fullscreen;
        DisplayFPSToggle.isOn = CurrentSettings.DisplayFPS;

        //AudioSliderChanged(0, CurrentSettings.MasterVolume);
        //AudioSliderChanged(1, CurrentSettings.SoundVolume);
        //AudioSliderChanged(2, CurrentSettings.MusicVolume);

        MasterVolumeSlider.Slider.onValueChanged.AddListener(MasterVolumeChanged);
        SoundVolumeSlider.Slider.onValueChanged.AddListener(SoundVolumeChanged);
        MusicVolumeSlider.Slider.onValueChanged.AddListener(MusicVolumeChanged);
        FPSSlider.Slider.onValueChanged.AddListener(FPSChanged);
        CamFOVSlider.Slider.onValueChanged.AddListener(CamFOVChanged);

        FullscreenToggle.onValueChanged.AddListener(FullscreenChanged);
        ResolutionDropdown.Dropdown.onValueChanged.AddListener(ResolutionChanged);
        QualityDropdown.Dropdown.onValueChanged.AddListener(QualityChanged);
        DisplayFPSToggle.onValueChanged.AddListener(DisplayFPSChanged);

        ApplyBtn.onClick.AddListener(() => { ApplyRequested(EventArguments[0]); });
        RevertBtn.onClick.AddListener(() => { DenyRequested(EventArguments[1]); });

        ExitBtn.onClick.AddListener(delegate { Main.SetActive(false); });
        Settings.SettingsChanged?.Invoke(CurrentSettings);
    }

    /// <summary>
    /// Match SettingsData.cs Settings Data with UI possibilities.
    /// </summary>
    private void ValidateData()
    {
        Settings.BaseSettings.MasterVolume = Mathf.Clamp(
            Settings.BaseSettings.MasterVolume,
            MasterVolumeSlider.Slider.minValue,
            MasterVolumeSlider.Slider.maxValue
        );

        Settings.BaseSettings.SoundVolume = Mathf.Clamp(
            Settings.BaseSettings.SoundVolume,
            SoundVolumeSlider.Slider.minValue,
            SoundVolumeSlider.Slider.maxValue
        );

        Settings.BaseSettings.MusicVolume = Mathf.Clamp(
            Settings.BaseSettings.MusicVolume,
            MusicVolumeSlider.Slider.minValue,
            MusicVolumeSlider.Slider.maxValue
        );
        
        Settings.BaseSettings.DisplayResolution = Mathf.Clamp(
            Settings.BaseSettings.DisplayResolution,
            0,
            Settings.Resolutions.Length - 1
        );

        Settings.BaseSettings.GameQuality = Mathf.Clamp(
            Settings.BaseSettings.GameQuality,
            0,
            QualityDropdown.Dropdown.options.Count
        );

        Settings.BaseSettings.FPS = Mathf.Clamp(
            Settings.BaseSettings.FPS,
            (int)FPSSlider.Slider.minValue,
            (int)FPSSlider.Slider.maxValue
        );

        Settings.BaseSettings.CamFOV = Mathf.Clamp(
            Settings.BaseSettings.CamFOV,
            (int)CamFOVSlider.Slider.minValue,
            (int)CamFOVSlider.Slider.maxValue
        );

        GameManager.Instance.FieldOfView = Settings.BaseSettings.CamFOV;
        CurrentSettings = Settings.BaseSettings;

        // Debug.Log(Settings.BaseSettings.FPS);
        // Debug.Log(Settings.BaseSettings.CamFOV);
    }

    /// <summary>
    /// Check possible display resolutions and setup resolutions dropdown.
    /// </summary>
    private void SetupResolutions()
    {
        List<string> options = new();
        Settings.Resolutions.Reverse();

        for (int i = 0; i < Settings.Resolutions.Length; i++)
        {
            string option = Settings.Resolutions[i].width + "x" + Settings.Resolutions[i].height 
                + " - @" + Settings.Resolutions[i].refreshRate + "hz";
            options.Add(option);
        }

        if (ResolutionDropdown.Dropdown.options.Count != 0)
            ResolutionDropdown.Dropdown.ClearOptions();

        ResolutionDropdown.Dropdown.AddOptions(options);
        ResolutionDropdown.Dropdown.RefreshShownValue();

        ResolutionDropdown.Dropdown.value = Settings.BaseSettings.DisplayResolution;
    }

    /// <summary>
    /// Toggle UI visibility.
    /// </summary>
    public void ToggleUI() => Main.SetActive(!Main.activeSelf);

    /// <summary>
    /// Initalize "(BaseSettings) CurrentSettings", AudioFrame and VideoFrame, as well as hook events + run private setup methods.
    /// [ Additional Methods: SetupResolutions(), ValidateData(), PostInitSetup() ]
    /// </summary>
    public void Initalize()
    {
        if (CurrentSettings == null)
            CurrentSettings = new();

        if (!AudioFrame.activeSelf)
            AudioFrame.SetActive(true);

        if (VideoFrame.activeSelf)
            VideoFrame.SetActive(false);
        
        AudioBtn.onClick.AddListener(delegate { ChangeSubFrame(true); });
        VideoBtn.onClick.AddListener(delegate { ChangeSubFrame(false); });

        SetupResolutions();
        ValidateData();
        PostInitSetup();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Serializable]
    public class EventArgs
    {
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public string Body { get; set; }
        [field: SerializeField] public Action<bool> Action { get; set; }
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

    private void ChangeSubFrame(bool isAudio)
    {
        GameObject target1 = isAudio ? AudioFrame : VideoFrame;
        GameObject target2 = !isAudio ? AudioFrame : VideoFrame;

        target1.SetActive(true);
        target2.SetActive(false);

        Title.text = "< Settings - " + target1.name + " >";
    }

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

    private float AudioSliderCalculations(float value) => Mathf.Log10(value) * 20;

    private void AudioSliderChanged(int valuePointer, float value) => AudioManager.Instance.AudioMixer.SetFloat(MixerValues[valuePointer], AudioSliderCalculations(value));

    private void MasterVolumeChanged(float value) { CurrentSettings.MasterVolume = value; AudioSliderChanged(0, value); }
    private void SoundVolumeChanged(float value) { CurrentSettings.SoundVolume = value; AudioSliderChanged(1, value); }
    private void MusicVolumeChanged(float value) { CurrentSettings.MusicVolume = value; AudioSliderChanged(2, value); }

    private void FPSChanged(float value) => CurrentSettings.FPS = (int)Mathf.Round(value);
    private void CamFOVChanged(float value) => CurrentSettings.CamFOV = (int)Mathf.Round(value);
    private void FullscreenChanged(bool value) => CurrentSettings.Fullscreen = value;
    private void ResolutionChanged(int value) => CurrentSettings.DisplayResolution = value;
    private void QualityChanged(int value) { CurrentSettings.GameQuality = value; QualitySettings.SetQualityLevel(value); }
    private void DisplayFPSChanged(bool value) { CurrentSettings.DisplayFPS = value; }

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

    public void ToggleUI() => Main.SetActive(!Main.activeSelf);

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

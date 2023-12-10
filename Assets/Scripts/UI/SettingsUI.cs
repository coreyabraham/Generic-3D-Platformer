using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Palmmedia.ReportGenerator.Core;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public class Target
    {
        public Button Button;
        public GameObject Frame;
    }

    [field: Header("Audio and Video Navigation")]
    [field: SerializeField] public Button AudioBtn { get; set; }
    [field: SerializeField] public Button VideoBtn { get; set; }

    [field: SerializeField] public GameObject AudioFrame { get; set; }
    [field: SerializeField] public GameObject VideoFrame { get; set; }

    [field: Header("Other Buttons")]
    [field: SerializeField] public Button ApplyBtn { get; set; }
    [field: SerializeField] public Button RevertBtn { get; set; }
    [field: SerializeField] public Button ExitBtn { get; set; }

    [field: Header("Setup")]
    [field: SerializeField] public TMP_Text Title { get; set; }
    [field: SerializeField] public List<Target> targets = new();
    [field: SerializeField] public SettingsData Settings { get; set; }

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

    [field: Header("Audio Mixing")]
    [field: SerializeField] public string[] MixerValues = new string[3];

    private BaseSettings CurrentSettings = new();

    private void ChangeSubFrame(bool isAudio)
    {
        GameObject target1 = isAudio ? AudioFrame : VideoFrame;
        GameObject target2 = !isAudio ? AudioFrame : VideoFrame;

        target1.SetActive(true);
        target2.SetActive(false);

        Title.text = "< Settings - " + target1.name + " >";
    }

    private float AudioSliderCalculations(float value) => Mathf.Log10(value) * 20;

    private void AudioSliderChanged(int ValuePointer, float value) => AudioManager.Instance.AudioMixer.SetFloat(MixerValues[ValuePointer], AudioSliderCalculations(value));

    private void MasterVolumeChanged(float value) { CurrentSettings.MasterVolume = value; AudioSliderChanged(0, value); }
    private void SoundVolumeChanged(float value) { CurrentSettings.SoundVolume = value; AudioSliderChanged(1, value); }
    private void MusicVolumeChanged(float value) { CurrentSettings.MusicVolume = value; AudioSliderChanged(2, value); }

    private void FPSChanged(float value) => CurrentSettings.FPS = (int)Mathf.Round(value);
    private void FullscreenChanged(bool value) => CurrentSettings.Fullscreen = value;
    private void ResolutionChanged(int value) => CurrentSettings.DisplayResolution = value;
    private void QualityChanged(int value) { CurrentSettings.GameQuality = value; QualitySettings.SetQualityLevel(value); }
    private void DisplayFPSChanged(bool value) { CurrentSettings.DisplayFPS = value; }

    private void PostInitSetup()
    {
        MasterVolumeSlider.Slider.value = Settings.BaseSettings.MasterVolume;
        SoundVolumeSlider.Slider.value = Settings.BaseSettings.SoundVolume;
        MusicVolumeSlider.Slider.value = Settings.BaseSettings.MusicVolume;
        FPSSlider.Slider.value = Settings.BaseSettings.FPS;

        MasterVolumeSlider.Input.text = Settings.BaseSettings.MasterVolume.ToString();
        SoundVolumeSlider.Input.text = Settings.BaseSettings.SoundVolume.ToString();
        MusicVolumeSlider.Input.text = Settings.BaseSettings.MusicVolume.ToString();
        FPSSlider.Input.text = Settings.BaseSettings.FPS.ToString();

        QualityDropdown.Dropdown.value = Settings.BaseSettings.GameQuality;
        QualitySettings.SetQualityLevel(Settings.BaseSettings.GameQuality);

        FullscreenToggle.isOn = Settings.BaseSettings.Fullscreen;
        DisplayFPSToggle.isOn = Settings.BaseSettings.DisplayFPS;

        CurrentSettings.MasterVolume = Settings.BaseSettings.MasterVolume;
        CurrentSettings.SoundVolume = Settings.BaseSettings.SoundVolume;
        CurrentSettings.MusicVolume = Settings.BaseSettings.MusicVolume;

        AudioSliderChanged(0, CurrentSettings.MasterVolume);
        AudioSliderChanged(1, CurrentSettings.SoundVolume);
        AudioSliderChanged(2, CurrentSettings.MusicVolume);

        CurrentSettings.DisplayResolution = Settings.BaseSettings.DisplayResolution;
        CurrentSettings.Fullscreen = Settings.BaseSettings.Fullscreen;

        MasterVolumeSlider.Slider.onValueChanged.AddListener(MasterVolumeChanged);
        SoundVolumeSlider.Slider.onValueChanged.AddListener(SoundVolumeChanged);
        MusicVolumeSlider.Slider.onValueChanged.AddListener(MusicVolumeChanged);
        FPSSlider.Slider.onValueChanged.AddListener(FPSChanged);

        FullscreenToggle.onValueChanged.AddListener(FullscreenChanged);
        ResolutionDropdown.Dropdown.onValueChanged.AddListener(ResolutionChanged);
        QualityDropdown.Dropdown.onValueChanged.AddListener(QualityChanged);
        DisplayFPSToggle.onValueChanged.AddListener(DisplayFPSChanged);

        // ApplyBtn.onClick.AddListener(() => { OnPromptFrame(ApplicableEvents[0].PromptTitle, ApplicableEvents[0].PromptBody, ApplicableEvents[0].EventName); });
        // RevertBtn.onClick.AddListener(() => { OnPromptFrame(ApplicableEvents[1].PromptTitle, ApplicableEvents[1].PromptBody, ApplicableEvents[1].EventName); });

        ExitBtn.onClick.AddListener(delegate { gameObject.SetActive(false); });
        Settings.SettingsChanged?.Invoke(Settings.BaseSettings);
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
        ResolutionDropdown.Dropdown.ClearOptions();

        ResolutionDropdown.Dropdown.value = Settings.BaseSettings.DisplayResolution;
    }

    public void Initalize()
    {
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

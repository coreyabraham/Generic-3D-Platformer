using System.Threading.Tasks;
using UnityEngine;

public enum StemType
{
    None = 0,
    SFX,
    Music
}

[CreateAssetMenu(menuName = "Audio/Stem", fileName = "Stem")]
public class Stem : ScriptableObject
{
    internal AudioSource source;
    public float CurrentVolume { 
        get { 
            return source.volume; 
        } 
            
        private set { 
            if (source) source.volume = value; 
        } 
    }

    [field: SerializeField] public AudioClip Clip { get; set; }
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public StemType ClipType { get; set; }
    [field: SerializeField] public float StartPosition { get; set; }
    [field: SerializeField] public bool IsLoopable { get; set; }
    [field: SerializeField, Range(-1, 1)] public float Pan { get; set; } = 0.0f;
    [field: SerializeField, Range(-3, 3)] public float Pitch { get; set; } = 1.0f;
    [field: SerializeField, Range(0, 1)] public float GlobalVolume { get; set; } = 0.5f;

    internal void OnEnable()
    {

    }

    internal void Play(bool overrideVolume = true)
    {
        if (source == null)
            return;

        SetSettings(overrideVolume);
        source.Play();
    }

    internal void PlayOneShot(bool overrideVolume = true)
    {
        if (source == null)
            return;

        SetSettings(overrideVolume);
        source.loop = false;
        source.Play();
    }

    internal void PlayAsSFX(bool overrideVolume = true)
    {
        if (source == null)
            return;

        SetSettings(overrideVolume);
        source.loop = false;
        source.PlayOneShot(Clip);
    }

    internal void Stop()
    {
        if (source == null)
            return;

        source.Stop();
    }

    internal void Pause()
    {
        if (source == null)
            return;

        source.Pause();
    }

    internal void Unpause()
    {
        if (source == null)
            return;

        source.UnPause();
    }

    internal async void FadeIn(float fadeTime = 1.0f, float resolution = 0.01f)
    {
        if (source == null)
            return;

        SetSettings(false);
        CurrentVolume = 0.0f;
        
        source.Play();
        await Fade(fadeTime, resolution);
    }

    internal async void FadeOut(float fadeTime = 1.0f, float resolution = 0.01f)
    {
        if (source == null)
            return;

        await Fade(fadeTime, resolution, false);
    }

    private void SetSettings(bool overrideCurrentVolume = true)
    {
        if (overrideCurrentVolume)
            CurrentVolume = GlobalVolume;

        source.pitch = Pitch;
        source.panStereo = Pan;

        source.time = StartPosition;
        source.loop = IsLoopable;
    }

    private async Task Fade(float fadeTime, float resolution, bool inFade = true)
    {
        float startVolume = CurrentVolume;

        float time = 0.0f;
        float step = 0.0f;

        float lerpVolume = GlobalVolume;
        if (!inFade)
            lerpVolume = 0.0f;

        while(step < 1)
        {
            if (time >= resolution)
            {
                step += resolution / fadeTime;
                CurrentVolume = Mathf.Lerp(startVolume, lerpVolume, step);
                time = 0.0f;
            }

            time += Time.deltaTime;
            await Task.Yield();
        }

        if (!inFade)
            Stop();
    }
}
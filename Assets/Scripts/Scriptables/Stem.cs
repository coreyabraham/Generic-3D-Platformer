using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An Audio Instance that is sorted into the "Audio Manager" system.
/// [ Uses: AudioManager.cs ]
/// </summary>
[CreateAssetMenu(menuName = "Game/Audio Stem", fileName = "Audio Stem")]
public class Stem : ScriptableObject
{
    // Get the source the stem will be tied to.
    internal AudioSource source;
    
    public float CurrentVolume { 
        get { 
            return source.volume; 
        } 
            
        private set { 
            if (source) source.volume = value; 
        } 
    }

    // Initalize all public variables that'll be read by "AudioManager.cs"
    [field: SerializeField] public AudioClip Clip { get; set; }
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public StemType ClipType { get; set; }
    [field: SerializeField] public float StartPosition { get; set; }
    [field: SerializeField] public bool IsLoopable { get; set; }
    
    
    // Provides more control than using vectors.
    [field: SerializeField, Range(-1, 1)] public float Pan { get; set; } = 0.0f;
    [field: SerializeField, Range(-3, 3)] public float Pitch { get; set; } = 1.0f;
    [field: SerializeField, Range(0, 1)] public float GlobalVolume { get; set; } = 0.5f;

    /// <summary>
    /// Update the "FriendlyName" string to the Stem's real name if empty.
    /// </summary>
    internal void OnEnable()
    {
        if (FriendlyName == string.Empty)
            FriendlyName = name;
    }

    /// <summary>
    /// Play the Audio Stem.
    /// </summary>
    /// <param name="overrideVolume"></param>
    internal void Play(bool overrideVolume = true)
    {
        if (source == null)
            return;

        SetSettings(overrideVolume);
        source.Play();
    }

    /// <summary>
    /// Play the Audio Stem in Oneshot Mode. (Loop will always be disabled)
    /// </summary>
    /// <param name="overrideVolume"></param>
    internal void PlayOneShot(bool overrideVolume = true)
    {
        if (source == null)
            return;

        SetSettings(overrideVolume);
        source.loop = false;
        source.Play();
    }

    /// <summary>
    /// Play the Audio Stem as a Sound Effect.
    /// </summary>
    /// <param name="overrideVolume"></param>
    internal void PlayAsSFX(bool overrideVolume = true)
    {
        if (source == null)
            return;

        SetSettings(overrideVolume);
        source.loop = false;
        source.PlayOneShot(Clip);
    }

    /// <summary>
    /// Stop the Audio Stem's Playback.
    /// </summary>
    internal void Stop()
    {
        if (source == null)
            return;

        source.Stop();
    }


    /// <summary>
    /// Pause the Audio Stem's Playback.
    /// </summary>
    internal void Pause()
    {
        if (source == null)
            return;

        source.Pause();
    }

    /// <summary>
    /// Unpause the Audio Stem's Playback.
    /// </summary>
    internal void Unpause()
    {
        if (source == null)
            return;

        source.UnPause();
    }

    /// <summary>
    /// Fade In the Audio Stem's Volume.
    /// </summary>
    /// <param name="fadeTime"></param>
    /// <param name="resolution"></param>
    internal async void FadeIn(float fadeTime = 1.0f, float resolution = 0.01f)
    {
        if (source == null)
            return;

        SetSettings(false);
        CurrentVolume = 0.0f;
        
        source.Play();
        await Fade(fadeTime, resolution);
    }
    
    /// <summary>
    /// Fade Out the Audio Stem's Volume.
    /// </summary>
    /// <param name="fadeTime"></param>
    /// <param name="resolution"></param>
    internal async void FadeOut(float fadeTime = 1.0f, float resolution = 0.01f)
    {
        if (source == null)
            return;

        await Fade(fadeTime, resolution, false);
    }

    /// <summary>
    /// Set common settings for the Audio Stem.
    /// </summary>
    /// <param name="overrideCurrentVolume"></param>
    private void SetSettings(bool overrideCurrentVolume = true)
    {
        if (overrideCurrentVolume)
            CurrentVolume = GlobalVolume;

        source.pitch = Pitch;
        source.panStereo = Pan;

        source.time = StartPosition;
        source.loop = IsLoopable;
    }

    /// <summary>
    /// Handle all Fade Volume related Tasks.
    /// </summary>
    /// <param name="fadeTime"></param>
    /// <param name="resolution"></param>
    /// <param name="inFade"></param>
    /// <returns></returns>
    private async Task Fade(float fadeTime, float resolution, bool inFade = true)
    {
        float startVolume = CurrentVolume;

        float time = 0.0f;
        float step = 0.0f;

        float lerpVolume = inFade ? GlobalVolume : 0.0f;

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
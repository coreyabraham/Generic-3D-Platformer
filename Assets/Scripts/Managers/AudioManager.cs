using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Handle Audio Game-Wide, including Sounds and Music.
/// [ Uses: Stem.cs ]
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Audio Manager : Instance / Singleton
    /// </summary>
    public static AudioManager Instance { get; internal set; }

    // Private Dictionary of all Audio Stems
    private Dictionary<string, Stem> Stems;

    // Initalize all Public Definitions
    [field: SerializeField] public GameObject AudioObject { get; set; }
    [field: SerializeField] public AudioMixer AudioMixer { get; set; }
    [field: SerializeField] public AudioMixerGroup MasterMixer { get; set; } = null;
    [field: SerializeField] public AudioMixerGroup MusicMixer { get; set; } = null;
    [field: SerializeField] public AudioMixerGroup SFXMixer { get; set; } = null;
    [field: SerializeField] public string[] AudioPaths { get; set; } // Relative to Resource Folder

    /// <summary>
    /// Associate the nulled Instance value to AudioManager.cs if not found.
    /// </summary>
    private void Awake() => Instance ??= this;

    /// <summary>
    /// Create a "Stems" Directory and check all Resource Directories (AudioPaths) to initalize all Audio Stems
    /// </summary>
    private void Start()
    {
        Stems = new();

        if (!AudioObject)
            Debug.LogWarning("No Audio Object has been set, meaning no audio will play!", this);

        List<Stem> stems = new();

        foreach (string path in AudioPaths)
            stems.AddRange(Resources.LoadAll<Stem>(path));

        foreach (Stem stem in stems)
        {
            AudioSource source = AudioObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = stem.Clip;
            stem.source = source;

            RouteStem(stem);

            if (stem.FriendlyName == string.Empty || Stems.ContainsKey(stem.FriendlyName))
                Stems.Add(stem.Clip.name, stem);
            else
                Stems.Add(stem.FriendlyName, stem);
        }
    }

    /// <summary>
    /// Route an Audio Stem to it's corrisponding StemType enum.
    /// </summary>
    /// <param name="stem"></param>
    private void RouteStem(Stem stem)
    {
        if (!MusicMixer || !SFXMixer)
            return;

        switch (stem.ClipType)
        {
            case StemType.Music: stem.source.outputAudioMixerGroup = MusicMixer; break;
            case StemType.SFX: stem.source.outputAudioMixerGroup = SFXMixer; break;
            default: break;
        }
    }
    
    /// <summary>
    /// Check if an Audio Stem is currerntly playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    internal bool IsPlaying(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) return stem.source.isPlaying;
        else return false;

    }

    /// <summary>
    /// Play an Audio Stem.
    /// </summary>
    /// <param name="clip"></param>
    internal void Play(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Play();
    }

    /// <summary>
    /// Play an Audio Stem as a Sound Effect.
    /// </summary>
    /// <param name="clip"></param>
    internal void PlaySFX(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.PlayAsSFX();
    }

    /// <summary>
    /// Stop an Audio Stem's Playback.
    /// </summary>
    /// <param name="clip"></param>
    internal void Stop(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Stop();
    }

    /// <summary>
    /// Pause an Audio Stem's Playback.
    /// </summary>
    /// <param name="clip"></param>
    internal void Pause(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Pause();
    }

    /// <summary>
    /// Unpause an Audio Stem's Playback.
    /// </summary>
    /// <param name="clip"></param>
    internal void UnPause(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Unpause();
    }

    /// <summary>
    /// Fade In the volume of an Audio Stem.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="timeInSeconds"></param>
    internal void FadeIn(string clip, float timeInSeconds = 2)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.FadeIn(timeInSeconds);
    }

    /// <summary>
    /// Fade Out the volume of an Audio Stem.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="timeInSeconds"></param>
    internal void FadeOut(string clip, float timeInSeconds = 2)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.FadeOut(timeInSeconds);
    }

    /// <summary>
    /// Crossfade an Audio Stem's volume.
    /// </summary>
    /// <param name="clipA"></param>
    /// <param name="clipB"></param>
    /// <param name="timeInSeconds"></param>
    internal void Crossfade(string clipA, string clipB, float timeInSeconds = 2) // ClipA is fade in, ClipB is fadeout
    {
        Stem stemA;
        Stem stemB;
        bool existsA = Stems.TryGetValue(clipA, out stemA);
        bool existsB = Stems.TryGetValue(clipB, out stemB);
        if (existsA && existsB)
        {
            stemA.FadeIn(timeInSeconds);
            stemB.FadeOut(timeInSeconds);
        }
    }
}

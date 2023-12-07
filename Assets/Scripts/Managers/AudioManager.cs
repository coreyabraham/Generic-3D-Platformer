using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; internal set; }

    public Dictionary<string, Stem> Stems;

    [field: SerializeField] public GameObject AudioObject { get; set; }
    [field: SerializeField] public AudioMixer AudioMixer { get; set; }
    [field: SerializeField] public AudioMixerGroup MusicMixer { get; set; } = null;
    [field: SerializeField] public AudioMixerGroup SFXMixer { get; set; } = null;
    [field: SerializeField] public string[] AudioPaths { get; set; } // Relative to Resource Folder

    private void Awake() => Instance ??= this;

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

    internal bool IsPlaying(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) return stem.source.isPlaying;
        else return false;

    }
    internal void Play(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Play();
    }

    internal void PlaySFX(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.PlayAsSFX();
    }

    internal void Stop(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Stop();
    }

    internal void Pause(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Pause();
    }

    internal void UnPause(string clip)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.Unpause();
    }

    internal void FadeIn(string clip, float timeInSeconds = 2)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.FadeIn(timeInSeconds);
    }

    internal void FadeOut(string clip, float timeInSeconds = 2)
    {
        Stem stem;
        bool exists = Stems.TryGetValue(clip, out stem);
        if (exists) stem.FadeOut(timeInSeconds);
    }


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

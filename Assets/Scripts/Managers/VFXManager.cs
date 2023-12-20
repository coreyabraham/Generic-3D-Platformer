using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage Visual Effects / Particle Systems
/// </summary>
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; internal set; }
    public Dictionary<string, VFX> VFXs;

    [field: SerializeField] public string[] VFXPaths { get; set; }

    /// <summary>
    /// Initalize "VFXPaths" Resource Directories and add VFX Keys
    /// </summary>
    private void Start()
    {
        Instance ??= this;

        VFXs = new();
        List<VFX> vfxs = new();

        foreach (string path in VFXPaths)
            vfxs.AddRange(Resources.LoadAll<VFX>(path));

        foreach (VFX vfx in vfxs)
        {
            if (vfx.FriendlyName == "" || VFXs.ContainsKey(vfx.FriendlyName))
                VFXs.Add(vfx.ParticlePrefab.name, vfx);
            else
                VFXs.Add(vfx.FriendlyName, vfx);
        }
    }

    /// <summary>
    /// Test
    /// </summary>
    /// <param name="vfxName"></param>
    /// <returns></returns>
    private (bool, VFX) CheckVFX(string vfxName)
    {
        VFX vfx;
        bool exists = VFXs.TryGetValue(vfxName, out vfx);

        return (exists, vfx);
    }

    internal void PlayAttached(string vfxName, GameObject attachment)
    {
        (bool, VFX) data = CheckVFX(vfxName);

        if (data.Item1)
            data.Item2.Play(attachment);
        else 
            return;
    }

    internal void Play(string vfxName, Transform position) => Play(vfxName, position.position);

    internal void Play(string vfxName, Vector3 position)
    {
        (bool, VFX) data = CheckVFX(vfxName);

        if (data.Item1)
            data.Item2.Play(position);
        else
            return;
    }
}

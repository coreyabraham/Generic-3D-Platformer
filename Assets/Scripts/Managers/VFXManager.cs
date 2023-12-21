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
    /// Check if VFX Exists, returns true and the VFX instance if found.
    /// </summary>
    /// <param name="vfxName"></param>
    /// <returns></returns>
    private (bool, VFX) CheckVFX(string vfxName)
    {
        VFX vfx;
        bool exists = VFXs.TryGetValue(vfxName, out vfx);

        return (exists, vfx);
    }

    /// <summary>
    /// Run the VFX associated with the name given.
    /// </summary>
    /// <param name="vfxName"></param>
    /// <param name="attachment"></param>
    internal void PlayAttached(string vfxName, GameObject attachment)
    {
        (bool, VFX) data = CheckVFX(vfxName);

        if (data.Item1)
            data.Item2.Play(attachment);
        else 
            return;
    }

    /// <summary>
    /// Run the VFX at a specific position (Transform).
    /// </summary>
    /// <param name="vfxName"></param>
    /// <param name="position"></param>
    internal void Play(string vfxName, Transform position) => Play(vfxName, position.position);

    /// <summary>
    /// Run the VFX at a specific position (Vector3).
    /// </summary>
    /// <param name="vfxName"></param>
    /// <param name="position"></param>
    internal void Play(string vfxName, Vector3 position)
    {
        (bool, VFX) data = CheckVFX(vfxName);

        if (data.Item1)
            data.Item2.Play(position);
        else
            return;
    }
}

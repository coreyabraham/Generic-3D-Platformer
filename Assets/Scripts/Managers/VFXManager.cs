using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; internal set; }

    [field: SerializeField] public Dictionary<string, VFX> VFXs;
    [field: SerializeField] public string[] VFXPaths { get; set; }

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

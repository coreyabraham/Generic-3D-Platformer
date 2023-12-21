using UnityEngine;

/// <summary>
/// A VFX Instance that is sorted into the "VFX Manager" script.
/// </summary>
[CreateAssetMenu(menuName = "Game/VFX", fileName = "VFX")]
public class VFX : ScriptableObject
{
    // Initalizing all Variables
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public GameObject ParticlePrefab { get; set; }
    [field: SerializeField] public bool DestroyAfter { get; set; }
    [field: SerializeField] public float AliveTime { get; set; }
    [field: SerializeField] public bool UseVFXAliveTime { get; set; }

    /// <summary>
    /// Play Visual Effects at a Transform's Position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>(GameObject) ParticlePrefab</returns>
    internal GameObject Play(Transform position) => Play(position.position);

    /// <summary>
    /// Play Visual Effects at a Vector3's Position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>(GameObject) ParticlePrefab</returns>
    internal GameObject Play(Vector3 position)
    {
        GameObject obj = Instantiate(ParticlePrefab);
        obj.transform.position = position;

        ParticleSystem part = obj.GetComponent<ParticleSystem>();

        if (UseVFXAliveTime)
            AliveTime = part.main.duration;

        if (DestroyAfter)
            Destroy(obj, AliveTime);

        part.Play();
        return obj;
    }

    /// <summary>
    /// Play Visual Effects on a GameObject
    /// </summary>
    /// <param name="parent"></param>
    /// <returns>(GameObject) ParticlePrefab</returns>
    internal GameObject Play(GameObject parent)
    {
        GameObject obj = Play(parent.transform);
        obj.transform.SetParent(parent.transform);
        return obj;
    }
}

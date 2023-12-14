using UnityEngine;

[CreateAssetMenu(menuName = "Game/VFX", fileName = "VFX")]
public class VFX : ScriptableObject
{
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public GameObject ParticlePrefab { get; set; }
    [field: SerializeField] public bool DestroyAfter { get; set; }
    [field: SerializeField] public float AliveTime { get; set; }

    internal GameObject Play(Transform position) => Play(position.position);
    internal GameObject Play(Vector3 position)
    {
        GameObject obj = Instantiate(ParticlePrefab);
        obj.transform.position = position;

        if (DestroyAfter)
            Destroy(obj, AliveTime);

        obj.GetComponent<ParticleSystem>().Play();
        return obj;
    }

    internal GameObject Play(GameObject parent)
    {
        GameObject obj = Play(parent.transform);
        obj.transform.SetParent(parent.transform);
        return obj;
    }
}

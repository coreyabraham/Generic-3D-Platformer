using UnityEngine;

public class LifeInstance : MonoBehaviour
{
    [field: SerializeField] public LifeTypes LifeType { get; set; }
    [field: SerializeField] public bool IncreaseValue { get; set; } = true;
    [field: SerializeField] private string VFXName { get; set; }
    
    public bool IsEnabled = true;

    public void ToggleUsablity()
    {
        IsEnabled = !IsEnabled;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = !mesh.enabled;
        
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.enabled = !collider.enabled;

        VFXManager.Instance.Play(VFXName, transform);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsEnabled || other.GetComponent<PlayerController>() == null)
            return;

        ToggleUsablity();
        LifeManager.Instance.ModifyLives(IncreaseValue, this);
    }
}

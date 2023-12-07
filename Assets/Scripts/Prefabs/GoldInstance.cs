using UnityEngine;

public class GoldInstance : MonoBehaviour
{
    [field: SerializeField] public GoldTypes GoldType { get; set; }
    [field: SerializeField] public bool IncreaseValue { get; set; } = true;
    [field: SerializeField] public bool IsEnabled = true;

    public void ToggleUsablity()
    {
        IsEnabled = !IsEnabled;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = !mesh.enabled;
        
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.enabled = !collider.enabled;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsEnabled || other.GetComponent<PlayerController>() == null)
            return;

        ToggleUsablity();
        GoldManager.Instance.ModifyGold(this, IncreaseValue);
    }
}

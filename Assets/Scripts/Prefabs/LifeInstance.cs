using UnityEngine;

/// <summary>
/// A player-collidable object that hooks into the Life Manager system.
/// [ Uses: LifeManager.cs ]
/// </summary>
public class LifeInstance : MonoBehaviour
{
    // Define all variables

    [field: SerializeField] public LifeTypes LifeType { get; set; }
    [field: SerializeField] public bool IncreaseValue { get; set; } = true;
    [field: SerializeField] private string VFXName { get; set; }
    
    public bool IsEnabled = true;

    /// <summary>
    /// Toggle the Usablity of the Instance (I.e; if it can be interacted with or not).
    /// </summary>
    public void ToggleUsablity()
    {
        IsEnabled = !IsEnabled;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = !mesh.enabled;
        
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.enabled = !collider.enabled;

        VFXManager.Instance.Play(VFXName, transform);
    }

    /// <summary>
    /// Toggles the Usability if this Instance is enabled and a Player is detected.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (!IsEnabled || other.GetComponent<PlayerController>() == null)
            return;

        ToggleUsablity();
        LifeManager.Instance.ModifyLives(IncreaseValue, this);
    }
}

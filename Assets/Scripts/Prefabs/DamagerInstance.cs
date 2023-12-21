using UnityEngine;

/// <summary>
/// Handle Player Death Collision.
/// </summary>
public class DamagerInstance : MonoBehaviour
{
    /// <summary>
    /// When the Player touches this instance, it should be set to a "Death" state only once.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null)
            return;

        GameManager.Instance.Player.TriggerDeath();
    }
}

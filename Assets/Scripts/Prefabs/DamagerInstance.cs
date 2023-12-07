using UnityEngine;

public class DamagerInstance : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null)
            return;

        GameManager.Instance.Player.TriggerDeath();
    }
}

using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    // Move this entire script into a "GenericCollider" class inheritence
    private bool Triggered;

    public void OnTriggerEnter(Collider other)
    {
        if (Triggered || other.GetComponent<PlayerController>() == null)
            return;

        Triggered = true;
        Debug.Log("COLLIDED!", this);
    }
}

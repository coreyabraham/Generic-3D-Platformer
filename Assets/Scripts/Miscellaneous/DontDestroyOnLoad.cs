using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move targeted objects to the "DontDestroyOnLoad" group, prevents deletion on new scene loading.
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    // Prepare a Dictionary of stored objects
    private static Dictionary<string, GameObject> Instantiated = new();

    private void Awake()
    {
        // If the object already exists then delete the duplicate.
        if (Instantiated.ContainsKey(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        // Call "DontDestroyOnLoad" on the object and instaniate it.
        DontDestroyOnLoad(gameObject);
        Instantiated.Add(gameObject.name, gameObject);
    }
}

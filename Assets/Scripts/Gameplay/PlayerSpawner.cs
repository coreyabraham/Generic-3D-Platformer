using UnityEngine;

/// <summary>
/// Spawn the player with the player prefab in the scene.
/// [ Searches for: PlayerController.cs ]
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    [field: Header("Assets")]
    [field: SerializeField] public GameObject PlayerPrefab { get; set; }
    [field: SerializeField] public Mesh DebugMesh { get; set; }
    [field: SerializeField] public Color DebugColor { get; set; } = Color.green;

    [field: Header("Offsets")]
    [field: SerializeField] public Vector3 Position { get; set; }

    /// <summary>
    /// Spawns player and applies positions + offsets as necessary.
    /// </summary>
    private void Start()
    {
        GameObject obj = Instantiate(PlayerPrefab);
        PlayerController player = obj.GetComponent<PlayerController>();

        // If "PlayerController" couldn't be found, return null.
        if (!player)
        {
            Destroy(obj);
            return;
        }

        player.transform.position = transform.position + Position;
        player.transform.rotation = transform.rotation;
        
        GameManager.Instance.Player = player;
    }

    /// <summary>
    /// Draw a Matrix to show a debug wireframe model in the Unity Editor
    /// [ Does not run during playback! ]
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // If "DebugMesh" was left empty on this method's action, then do nothing.
        if (DebugMesh == null)
            return;

        // Apply Matrix, Colour and Draw the Mesh
        Gizmos.matrix = Matrix4x4.TRS(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform.lossyScale);
        Gizmos.color = DebugColor;
        Gizmos.DrawWireMesh(DebugMesh);
    }
}

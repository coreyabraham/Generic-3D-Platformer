using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [field: Header("Assets")]
    [field: SerializeField] public GameObject PlayerPrefab { get; set; }
    [field: SerializeField] public Mesh DebugMesh { get; set; }

    [field: Header("Offsets")]
    [field: SerializeField] public Vector3 Position { get; set; }

    private void Start()
    {
        GameObject obj = Instantiate(PlayerPrefab);
        PlayerController player = obj.GetComponent<PlayerController>();

        if (!player)
        {
            Destroy(obj);
            return;
        }

        player.transform.position = transform.position + Position;
        player.transform.rotation = transform.rotation;
        
        GameManager.Instance.Player = player;
        // InputManager.Instance.EnabledControls();
    }

    private void OnDrawGizmosSelected()
    {
        if (DebugMesh == null)
            return;

        Gizmos.matrix = Matrix4x4.TRS(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform.lossyScale);
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(DebugMesh);
    }
}

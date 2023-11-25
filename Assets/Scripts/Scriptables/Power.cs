using UnityEngine;

[CreateAssetMenu(menuName = "Game/Powerup", fileName = "Powerup")]
public class Power : ScriptableObject
{
    [field: Header("Modifiers")]
    [field: SerializeField] public int JumpModifier { get; set; }
    [field: SerializeField] public int SpeedModifier { get; set; }

    [field: Header("Miscellaneous")]
    [field: SerializeField] public ParticleSystem Particles { get; set; }
}

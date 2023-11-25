using UnityEngine;

public class PlayerData
{
    public static PlayerData Instance { get; internal set; }

    private void Awake() => Instance ??= this;
}

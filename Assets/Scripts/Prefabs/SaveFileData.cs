using System;

/// <summary>
/// Base Class defining all applicable Save File Data.
/// </summary>
[Serializable]
public class SaveFileData
{
    public string FileName;
    public string FriendlyName;
    public string LevelName;
    public int GoldCount;
    public int LivesCount;
}

using System;

[Serializable]
public class SaveSlotInfo
{
    #region Fields

    public int SaveSlotId;
    public int UserId;

    public string Username;
    public string SaveSlotName;

    public int Level;
    public int Score;
    public int ProgressPercent;

    public string LastPlayedUtc;

    #endregion
}
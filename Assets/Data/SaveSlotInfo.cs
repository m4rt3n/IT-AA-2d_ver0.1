using System;

[Serializable]
public class SaveSlotInfo
{
    public int SaveSlotId;
    public int UserId;

    public string Username;
    public string SaveSlotName;

    public int Level;
    public int Score;
    public string LastPlayedUtc;

    public int ProgressPercent;
}
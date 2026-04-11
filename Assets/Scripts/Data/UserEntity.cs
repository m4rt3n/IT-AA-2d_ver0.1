using System;

[Serializable]
public class UserEntity
{
    public int Id;
    public string Username;
    public string PasswordHash;
    public string PasswordSalt;
    public int LastUsedSaveSlotId;
}
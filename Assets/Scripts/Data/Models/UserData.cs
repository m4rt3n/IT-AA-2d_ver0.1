using System;

namespace ITAA.Data.Models
{
    [Serializable]
    public class UserData
    {
        public int UserId;
        public string Username;
        public string Password;

        public UserData Clone()
        {
            return new UserData
            {
                UserId = UserId,
                Username = Username,
                Password = Password
            };
        }
    }
}
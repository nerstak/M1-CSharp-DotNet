using System;

namespace Communication.model
{
    [Serializable]
    public class User: DataPacket, Recipient
    {
        private String username;
        private String password;

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string Password
        {
            set => password = value;
        }

        public bool CheckPassword(String pwd)
        {
            return password.Equals(pwd);
        }
    }
}
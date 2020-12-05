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
            get => password;
            set => password = value;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return Equals((User) obj);
            }
        }

        protected bool Equals(User other)
        {
            return username == other.username && password == other.password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(username, password);
        }
    }
}
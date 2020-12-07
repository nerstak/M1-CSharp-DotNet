using System;

namespace Communication.model
{
    [Serializable]
    public class User: DataPacket, Recipient
    {
        private String _username;
        private String _password;

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public override string ToString()
        {
            return "@" + _username;
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
            return _username == other._username && _password == other._password;
        }
    }
}
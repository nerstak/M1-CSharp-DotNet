using System;

namespace Communication.model
{
    /// <summary>
    /// User
    /// </summary>
    [Serializable]
    public class User: IDataPacket, IRecipient, IEquatable<User>
    {
        private String _username;
        private String _password;

        public User() {}

        public User(User u)
        {
            _username = u._username;
        }
        
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
            return this.Equals(obj as User);
        }

        public bool Equals(User other)
        {
            // Check for null obj
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for common success case
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            // Checking runtime type
            if (this.GetType() != other.GetType())
            {
                return false;
            }
            
            // Return true if fields match
            return (_username == other._username) && (_password == other._password);
        }

        public static bool operator ==(User lhs, User rhs)
        {
            // Check for null on left side.
            if (object.ReferenceEquals(lhs, null))
            {
                if (object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(User lhs, User rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return (_username != null ? _username.GetHashCode() : 0);
        }
    }
}
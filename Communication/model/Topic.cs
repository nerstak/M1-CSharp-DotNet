using System;

namespace Communication.model
{
    /// <summary>
    /// Topic class
    /// </summary>
    [Serializable]
    public class Topic: IDataPacket, IRecipient
    {
        private readonly String _name;

        public Topic(String name)
        {
            _name = name;
        }

        public string Name => _name;

        public override string ToString()
        {
            return _name;
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
                return Equals((Topic) obj);
            }
        }

        protected bool Equals(Topic other)
        {
            return _name == other._name;
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }
    }
}
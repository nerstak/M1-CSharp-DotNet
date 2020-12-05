using System;

namespace Communication.model
{
    [Serializable]
    public class Topic: DataPacket, Recipient
    {
        private readonly String _name;

        public Topic(String name)
        {
            this._name = name;
        }

        public string Name => _name;

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
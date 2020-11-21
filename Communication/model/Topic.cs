using System;

namespace Communication.model
{
    [Serializable]
    public class Topic: DataPacket, Recipient
    {
        private String name;

        public Topic(String name)
        {
            this.name = name;
        }

        public string Name
        {
            get => name;
        }
    }
}
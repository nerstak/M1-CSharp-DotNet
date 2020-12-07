using System;

namespace Communication.model
{
    [Serializable]
    public class InformationMessage: DataPacket
    {
        private string content;

        public string Content => content;

        public InformationMessage(string content)
        {
            this.content = content;
        }

        public override string ToString()
        {
            return content;
        }
    }
}
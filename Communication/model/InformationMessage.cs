using System;

namespace Communication.model
{
    /// <summary>
    /// Information message
    /// </summary>
    [Serializable]
    public class InformationMessage: IDataPacket
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
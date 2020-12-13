using System;

namespace Communication.model
{
    /// <summary>
    /// Message sent from an user to topic or user
    /// </summary>
    [Serializable]
    public class Message: IDataPacket
    {
        private String _content;
        private User _sender; // Only an user can send a message
        private IRecipient _recipient;

        public Message(string content, User sender, IRecipient recipient)
        {
            _content = content;
            _sender = sender;
            _recipient = recipient;
        }

        public string Content
        {
            get => _content;
        }

        public User Sender
        {
            get => _sender;
        }

        public IRecipient Recipient
        {
            get => _recipient;
        }

        public override string ToString()
        {
            return _sender + " to " + _recipient + ": " + _content;
        }
    }
}
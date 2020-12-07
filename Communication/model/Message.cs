using System;

namespace Communication.model
{
    [Serializable]
    public class Message: DataPacket
    {
        private String content;
        private User sender; // Only an user can send a message
        private Recipient recipient;

        public Message(string content, User sender, Recipient recipient)
        {
            this.content = content;
            this.sender = sender;
            this.recipient = recipient;
        }

        public string Content
        {
            get => content;
        }

        public User Sender
        {
            get => sender;
        }

        public Recipient Recipient
        {
            get => recipient;
        }

        public override string ToString()
        {
            return sender + " to " + recipient + ": " + content;
        }
    }
}
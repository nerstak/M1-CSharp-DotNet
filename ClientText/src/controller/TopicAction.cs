using System;
using Communication.model;
using Communication.utils;

namespace ClientText.controller
{
    /// <summary>
    /// Actions relative to topics
    /// </summary>
    public class TopicAction: AbstractAction
    {
        /// <summary>
        /// Create a topic
        /// </summary>
        /// <param name="name">Name of topic</param>
        /// <param name="message">Information message</param>
        /// <returns>Validity of operation</returns>
        public bool CreateTopic(string name, out string message)
        {
            message = null;
            var t = new Topic(name);

            CustomPacket = new CustomPacket(Operation.CreateTopic, t);
            try
            {
                Net.sendMsg(Connection.GetStream(), CustomPacket);

                CustomPacket = Net.rcvMsg(Connection.GetStream());
                message = GetInformationMessage();
                if (CustomPacket.OperationOrder == Operation.Reception)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                message = "Connection failed";
                Console.Out.WriteLine("Error: " + e);
            }
            
            return false;
        }

        /// <summary>
        /// List all topics
        /// </summary>
        /// <param name="message">Information message</param>
        /// <returns>List of topics (may be null)</returns>
        public TopicList ListTopics(out string message)
        {
            message = null;
            CustomPacket = new CustomPacket(Operation.ListTopics, null);
            try
            {
                Net.sendMsg(Connection.GetStream(), CustomPacket);

                CustomPacket = Net.rcvMsg(Connection.GetStream());
                
                if (CustomPacket.OperationOrder == Operation.Reception && (CustomPacket.Data) is TopicList list)
                {
                    return list;
                }
                throw new Exception("Wrong type given");
            }
            catch (Exception e)
            {
                message = "Connection failed";
                Console.Out.WriteLine("Error: " + e);
            }

            return null;
        }
    }
}
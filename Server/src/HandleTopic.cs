using Communication.model;

namespace Server
{
    public partial class Receiver
    {
        /// <summary>
        /// Create topic
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket CreateTopic(CustomPacket customPacket)
        {
            var t = (Topic) customPacket.Data;
            Server.TopicList.Semaphore.WaitOne();
            if (Server.TopicList.SearchTopic(t) == null)
            {
                var connectedUsersTopic = new ConnectedUsersTopic(t);
                Server.TopicList.List.Add(connectedUsersTopic);
                Server.TopicList.Semaphore.Release();
                return new CustomPacket(Operation.Reception, new InformationMessage("Topic created"));
            }
            else
            {
                Server.TopicList.Semaphore.Release();
                return
                    new CustomPacket(Operation.Refused, new InformationMessage("Topic already existing"));
            }
        }

        /// <summary>
        /// List topics
        /// </summary>
        /// <returns>Packet to send</returns>
        private CustomPacket ListTopics()
        {
            return new CustomPacket(Operation.Reception, Server.TopicList.GetListTopics());
        }

        /// <summary>
        /// Join a topic
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket JoinTopic(CustomPacket customPacket)
        {
            Operation op = Operation.Reception;
            string message = "Added to topic " + (Topic) customPacket.Data;
            var topicList = Server.TopicList.SearchTopic((Topic) customPacket.Data);
            
            // We check if the topic exist
            if (topicList != null)
            {
                // We check if the user can join this topic
                if (topicList.UserList.SearchUser(_user) == null)
                {
                    topicList.UserList.AddUser(_user);
                }
                else
                {
                    op = Operation.Refused;
                    message = "You are already in " + (Topic) customPacket.Data;
                }
            }
            else
            {
                op = Operation.Refused;
                message = (Topic) customPacket.Data + " does not exists";
            }
            
            return new CustomPacket(op, new InformationMessage(message));
        }
    }
}
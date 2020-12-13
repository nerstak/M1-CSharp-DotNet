using Communication.model;
using Server.model;

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
                Server.TopicList.List.Add(t, new UserList());
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
                if (topicList.SearchUser(_user) == null)
                {
                    topicList.AddUser(_user);
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

        /// <summary>
        /// Leave a topic
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket LeaveTopic(CustomPacket customPacket)
        {
            Operation op = Operation.Reception;
            string message = "Disconnected from topic " + (Topic) customPacket.Data;
            var topicList = Server.TopicList.SearchTopic((Topic) customPacket.Data);

            // We check if the topic exist
            if (topicList != null)
            {
                // We check if the user can join this topic
                if (topicList.SearchUser(_user) != null)
                {
                    topicList.RemoveUser(_user);
                }
                else
                {
                    op = Operation.Refused;
                    message = "You are not in " + (Topic) customPacket.Data;
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
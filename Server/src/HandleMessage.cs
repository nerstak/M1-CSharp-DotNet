using System.Net.Sockets;
using Communication.model;
using Communication.utils;

namespace Server
{
    public partial class Receiver
    {
        /// <summary>
        /// Send message to topic
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket SendToTopic(CustomPacket customPacket)
        {
            Message msg = (Message) customPacket.Data;
            
            // Checking if user is connected to the topic
            if (Server.TopicList.CheckUserConnectionTopic((Topic) msg.Recipient, msg.Sender))
            {
                // Sending the message to every user
                var users = Server.TopicList.SearchTopic((Topic) msg.Recipient).UserList.Users;

                CustomPacket pck = new CustomPacket(Operation.Reception, msg);
                foreach (var u in users)
                {
                    TcpClient tmp = Server.UserConnections[u];
                    Net.sendMsg(tmp.GetStream(),pck);
                }
                
                return new CustomPacket(Operation.Reception,new InformationMessage("Message sent"));
            }
            
            return new CustomPacket(Operation.Refused, new InformationMessage("You are not connected to " + (Topic) msg.Recipient));
        }
        
        /// <summary>
        /// Send message to user
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket SendToUser(CustomPacket customPacket)
        {
            Message msg = (Message) customPacket.Data;

            User recipient = Server.ConnectedUsers.SearchUsername(((User) msg.Recipient).Username);
            
            // Checking if receiving user is connected
            if (recipient != null) ;
            {
                // Sending the message to both users
                CustomPacket pck = new CustomPacket(Operation.Reception, msg);
                
                Net.sendMsg(Server.UserConnections[recipient].GetStream(),pck);
                Net.sendMsg(comm.GetStream(),pck);
                
                return new CustomPacket(Operation.Reception,new InformationMessage("Message sent"));
            }
            
            return new CustomPacket(Operation.Refused, new InformationMessage("You are not connected to " + (Topic) msg.Recipient));
        }
    }
}
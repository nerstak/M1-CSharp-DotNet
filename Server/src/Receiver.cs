using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Communication.utils;
using Communication.model;
using Server.model;

namespace Server
{
    public partial class Receiver
    {
        private TcpClient comm;
        private bool _keepAlive = true;
        private User _user = null;

        public Receiver(TcpClient s)
        {
            comm = s;
        }

        /// <summary>
        /// Listening function
        /// </summary>
        public void Listener()
        {
            try
            {
                while (_keepAlive)
                {
                    CustomPacket customPacket = Net.rcvMsg(comm.GetStream());
                    CustomPacket toSend = null;
                    if (_user == null)
                    {
                        switch (customPacket.Operation)
                        {
                            case Operation.CreateUser:
                                toSend = CreateUser(customPacket);
                                break;
                            case Operation.LoginUser:
                                toSend = LoginUser(customPacket);
                                break;
                        }
                    }
                    else
                    {
                        switch (customPacket.Operation)
                        {
                            case Operation.ListTopics:
                                toSend = ListTopics();
                                break;
                            case Operation.CreateTopic:
                                toSend = CreateTopic(customPacket);
                                break;
                            case Operation.JoinTopic:
                                toSend = JoinTopic(customPacket);
                                break;
                            case Operation.SendToTopic:
                                toSend = SendToTopic(customPacket);
                                break;
                            case Operation.SendToUser:
                                toSend = SendToUser(customPacket);
                                break;
                        }
                        
                    }
                    
                    Net.sendMsg(comm.GetStream(),toSend);
                }
            }
            catch (Exception)
            {
                // If we have trouble, we disconnect the user
                _keepAlive = false;
                if (_user != null)
                {
                    Disconnecting();
                }
                Console.WriteLine("Connection lost");
            }
        }

        /// <summary>
        /// Connecting user
        /// </summary>
        private void Connecting()
        {
            CustomPacket pck = new CustomPacket(Operation.Reception, 
                new InformationMessage(_user.Username + " is now connected"));
            Broadcast(pck, Server.ConnectedUsers);
        }

        /// <summary>
        /// Disconnecting user
        /// </summary>
        private void Disconnecting()
        {
            Server.TopicList.RemoveUserFromAll(_user);
            Server.ConnectedUsers.RemoveUser(_user);
            Server.UserConnections.Remove(_user);
            
            // Disconnect message
            CustomPacket pck = new CustomPacket(Operation.Reception, 
                new InformationMessage(_user.Username + " is now offline"));
            Broadcast(pck, Server.ConnectedUsers);
        }

        /// <summary>
        /// Send a packet to every user of a list
        /// </summary>
        /// <param name="pck">Packet to send</param>
        /// <param name="userList">Users concerned</param>
        private void Broadcast(CustomPacket pck, UserList userList)
        {
            var users = userList.Users;
            
            foreach (var u in users)
            {
                TcpClient tmp = Server.UserConnections[u];
                Net.sendMsg(tmp.GetStream(),pck);
            }
        }
    }
}
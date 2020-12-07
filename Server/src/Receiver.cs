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
                    Server.TopicList.RemoveUserFromAll(_user);
                    Server.ConnectedUsers.RemoveUser(_user);
                    Server.UserConnections.Remove(_user);
                }
                Console.WriteLine("Connection lost");
            }
        }
    }
}
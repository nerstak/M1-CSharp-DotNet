using System;
using System.Net.Sockets;
using Communication.model;
using Communication.utils;
using Server.model;

namespace Server
{
    /// <summary>
    /// Receiver class is dedicated to handling actions of only one user (one user per thread)
    /// </summary>
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
                        // Actions for non logged users
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
                        // Actions for logged in users
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
                            case Operation.LeaveTopic:
                                toSend = LeaveTopic(customPacket);
                                break;
                            case Operation.SendToTopic:
                                toSend = SendToTopic(customPacket);
                                break;
                            case Operation.SendToUser:
                                toSend = SendToUser(customPacket);
                                break;
                        }
                    }

                    if (toSend != null)
                    {
                        Net.sendMsg(comm.GetStream(), toSend);
                    }
                }
            }
            catch (Exception)
            {
                // If we have trouble, we disconnect the user
                _keepAlive = false;
                string message = "Connection lost";
                if (_user != null)
                {
                    message += " with " + _user;
                    Disconnecting();
                }

                Console.WriteLine(message);
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
            Server.TcpClients.Remove(_user);

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
                TcpClient tmp = Server.TcpClients[u];
                Net.sendMsg(tmp.GetStream(), pck);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Communication.utils;
using Communication.model;
using Server.model;

namespace Server
{
    class Receiver
    {
        private TcpClient comm;
        private bool _keepAlive = true;
        private User _user = null;

        public Receiver(TcpClient s)
        {
            comm = s;
        }

        public void doOperation()
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
                        }
                        
                    }
                    
                    Net.sendMsg(comm.GetStream(),toSend);
                }
            }
            catch (Exception ex)
            {
                _keepAlive = false;
                if (_user != null)
                {
                    Server.ConnectedUsers.RemoveUser(_user);
                    Server.UserConnections.Remove(_user);
                }
                Console.WriteLine("Connection lost");
            }
        }

        /// <summary>
        /// Create an user
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket CreateUser(CustomPacket customPacket)
        {
            var u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            
            // We add the user if it does not already exists
            if (Server.AllUsers.SearchUser(u) == null)
            {
                Server.AllUsers.AddUser(u);
                Server.AllUsers.Semaphore.Release();
                return new CustomPacket(Operation.Reception, new InformationMessage("Account created"));
            }
            else
            {
                Server.AllUsers.Semaphore.Release();
                return
                    new CustomPacket(Operation.Refused, new InformationMessage("User already existing"));
            }
        }

        /// <summary>
        /// Log an user
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket LoginUser(CustomPacket customPacket)
        {
            User u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            var searchedUser = Server.AllUsers.SearchUser(u);
            Server.AllUsers.Semaphore.Release();

            string errorMessage = "";
            if (searchedUser != null) // No need to check for credentials, SearchUser already did it
            {
                if (Server.ConnectedUsers.SearchUser(searchedUser) == null) // No double connection
                {
                    Server.ConnectedUsers.Semaphore.WaitOne();
                    Server.ConnectedUsers.AddUser(u);
                    _user = u;
                    Server.UserConnections.Add(_user,comm);
                    Server.ConnectedUsers.Semaphore.Release();
                    return new CustomPacket(Operation.Reception, new InformationMessage("Connected"));
                }

                errorMessage = "You are already connected";
            }
            else
            {
                errorMessage = "Wrong credentials";
            }
            
            
            return new CustomPacket(Operation.Refused, new InformationMessage(errorMessage));
        }

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

        private CustomPacket JoinTopic(CustomPacket customPacket)
        {
            Operation op = Operation.Reception;
            string message = "Added to topic " + (Topic) customPacket.Data;
            var topicList = Server.TopicList.SearchTopic((Topic) customPacket.Data);
            if (topicList != null)
            {
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

        private CustomPacket SendToTopic(CustomPacket customPacket)
        {
            Message msg = (Message) customPacket.Data;
            if (Server.TopicList.CheckUserConnectionTopic((Topic) msg.Recipient, msg.Sender))
            {
                var users = Server.TopicList.List.Find(tp => tp.Topic.Equals(msg.Recipient))?.UserList.Users;
                foreach (var u in users)
                {
                    TcpClient tmp = Server.UserConnections[u];
                    Net.sendMsg(tmp.GetStream(),new CustomPacket(Operation.Reception,msg));
                }
                
                return new CustomPacket(Operation.Reception,null);
            }
            
            return new CustomPacket(Operation.Refused, new InformationMessage("You are not connected to " + (Topic) msg.Recipient));
        }
    }
}
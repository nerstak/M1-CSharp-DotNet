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
        private bool connected = false;

        public Receiver(TcpClient s)
        {
            comm = s;
        }

        public void doOperation()
        {
            try
            {
                while (true)
                {
                    CustomPacket customPacket = Net.rcvMsg(comm.GetStream());
                    CustomPacket toSend = null;
                    if (connected == false)
                    {
                        switch (customPacket.OperationOrder)
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
                        switch (customPacket.OperationOrder)
                        {
                            case Operation.ListTopics:
                                toSend = ListTopics();
                                break;
                            case Operation.CreateTopic:
                                toSend = CreateTopic(customPacket);
                                break;
                        }
                        
                    }
                    
                    Net.sendMsg(comm.GetStream(),toSend);
                }
            }
            catch (Exception ex)
            {
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
            if (searchedUser != null && // No need to check for credentials, SearchUser already did it
                Server.ConnectedUsers.SearchUser(searchedUser) == null)  // No double connection
            {
                Server.ConnectedUsers.Semaphore.WaitOne();
                Server.ConnectedUsers.AddUser(u);
                connected = true;
                Server.ConnectedUsers.Semaphore.Release();
                return new CustomPacket(Operation.Reception, new InformationMessage("Connected"));
            }
            else
            {
                Console.WriteLine("Wrong password!");
                return
                    new CustomPacket(Operation.Refused, new InformationMessage("Wrong credentials"));
            }
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
                Server.TopicList.ConnectedUsersTopics.Add(connectedUsersTopic);
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
    }
}
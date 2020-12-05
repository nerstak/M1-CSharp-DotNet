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
                                toSend = ListTopics(customPacket);
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
                Console.WriteLine(ex.ToString());
            }
        }

        /**
         * Create an user
         */
        private CustomPacket CreateUser(CustomPacket customPacket)
        {
            var u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
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

        private CustomPacket LoginUser(CustomPacket customPacket)
        {
            User u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            u = Server.AllUsers.SearchUser(u);
            Server.AllUsers.Semaphore.Release();
            if (u != null)
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

        private CustomPacket ListTopics(CustomPacket customPacket)
        {
            return new CustomPacket(Operation.Reception, Server.TopicList.GetListTopics());
        }
    }
}
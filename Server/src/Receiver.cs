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
                                toSend = createUser(customPacket);
                                break;
                            case Operation.LoginUser:
                                toSend = loginUser(customPacket);
                                break;
                            case Operation.ListTopics:
                                toSend = listTopics(customPacket);
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
        private CustomPacket createUser(CustomPacket customPacket)
        {
            var u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            if (Server.AllUsers.SearchUser(u) == null)
            {
                Server.AllUsers.AddUser(u);
                Console.Out.WriteLine(Server.AllUsers.ToString());
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

        private CustomPacket loginUser(CustomPacket customPacket)
        {
            User u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            u = Server.AllUsers.SearchUser(u);
            Server.AllUsers.Semaphore.Release();
            if (u != null)
            {
                Server.ConnectedUsers.Semaphore.WaitOne();
                u = Server.ConnectedUsers.SearchUser(u);
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

        private CustomPacket listTopics(CustomPacket customPacket)
        {
            return new CustomPacket(Operation.Reception, Server.TopicList.GetListTopics());
        }
    }
}
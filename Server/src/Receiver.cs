using System;
using System.Net.Sockets;
using Communication;
using Communication.model;

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
                                break;
                            case Operation.LoginUser:
                                toSend = loginUser(customPacket);
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

        private CustomPacket loginUser(CustomPacket customPacket)
        {
            User u = (User) customPacket.Data;
            u = Server.UsersList.SearchUser(u);
            if (u != null)
            {
                Console.WriteLine("Connected!");
                return new CustomPacket(Operation.Reception, new InformationMessage("Connected"));
            }
            else
            {
                Console.WriteLine("Wrong password!");
                return
                    new CustomPacket(Operation.Refused, new InformationMessage("Wrong credentials"));
            }
        }
    }
}
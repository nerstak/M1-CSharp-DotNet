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
                    if (connected == false && customPacket.OperationOrder == Operation.LoginUser)
                    {
                        User u = (User) customPacket.Data;
                        u = Server.UsersList.SearchUser(u);
                        if (u != null)
                        {
                            Console.WriteLine("Connected!");
                            Net.sendMsg(comm.GetStream(),
                                new CustomPacket(Operation.Reception, new InformationMessage("Connected")));
                        }
                        else
                        {
                            Console.WriteLine("Wrong password!");
                            Net.sendMsg(comm.GetStream(),
                                new CustomPacket(Operation.Refused, new InformationMessage("Wrong credentials")));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
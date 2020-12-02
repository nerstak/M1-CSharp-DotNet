using System;
using System.Net.Sockets;
using Communication;
using Communication.model;

namespace ClientText
{
    class Client
    {
        private string hostname;
        private int port;

        public Client(string h, int p)
        {
            hostname = h;
            port = p;
        }

        public void Start()
        {
            TcpClient comm = new TcpClient(hostname, port);
            Console.WriteLine("Connection established");
            while (true)
            {
                try
                {
                    User u = new User();
                    Console.Write("Username: ");
                    u.Username = Console.ReadLine();
                    Console.WriteLine("\nPassword: ");
                    u.Password = Console.ReadLine();

                    CustomPacket cP = new CustomPacket(Operation.LoginUser, u);

                    // To send
                    Net.sendMsg(comm.GetStream(), cP);
                    // Result
                    Console.WriteLine(((InformationMessage) Net.rcvMsg(comm.GetStream()).Data).Content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                

            }
        }

    }
}
using System;
using System.Net.Sockets;

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
                /*
                string expr = Console.ReadLine();
                Console.WriteLine(expr);
                */
                
                // To send
                // Net.sendMsg(comm.GetStream(), new Expr(op1, op2, op));
                // Result
                // Console.WriteLine("Result = " + (Result)Net.rcvMsg(comm.GetStream()));
              
            }
        }

    }
}
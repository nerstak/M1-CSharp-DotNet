using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class Server
    {
        private int port;

        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] {127, 0, 0, 1}), port);
            l.Start();

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm);
                //new Thread(new Receiver(comm).doOperation).Start();
            }
        }

        class Receiver
        {
            private TcpClient comm;

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void doOperation()
            {
                Console.WriteLine("Computing operation");
                while (true)
                {
                    // read expression
                    // Expr msg = (Expr)Net.rcvMsg(comm.GetStream());

                    // Console.WriteLine("expression received");
                    // send result
                    // Do something with message
                }
            }
        }
    }
}
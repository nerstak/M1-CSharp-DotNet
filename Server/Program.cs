using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server serv = new Server(8976);
            serv.Start();
        }
    }
}
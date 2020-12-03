using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication.model;

namespace Server
{
    public class Server
    {
        private int port;
        private static ConnectedUsersTopic usersList;
        /// <summary>
        /// Semaphore for list access
        /// </summary>
        private static Semaphore semaphoreList = new Semaphore(1, 1);

        public static ConnectedUsersTopic UsersList
        {
            get => usersList;
        }

        public static Semaphore SemaphoreList => semaphoreList;

        public Server(int port)
        {
            this.port = port;
            usersList = new ConnectedUsersTopic(null);
            /*User u = new User();
            u.Username = "admin";
            u.Password = "password";
            users.AddUser(u);
            users.SaveUsers(@"..\..\..\savedFiles\users.bin");*/
            usersList.LoadUsers(@"..\..\..\savedFiles\users.bin");
        }

        public void Start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] {127, 0, 0, 1}), port);
            l.Start();

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm);
                new Thread(new Receiver(comm).doOperation).Start();
            }
        }
        
        
    }
}
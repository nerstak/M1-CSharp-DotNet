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
        private static UserList _allUsers;
        private static UserList _connectedUsers;
        
        public static UserList AllUsers => _allUsers;
        public static UserList ConnectedUsers => _connectedUsers;



        public Server(int port)
        {
            this.port = port;
            _allUsers = new UserList(@"..\..\..\savedFiles\users.bin");
            _allUsers.LoadUsers();
            
            _connectedUsers = new UserList();
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
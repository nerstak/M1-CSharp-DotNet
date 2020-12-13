using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication.model;
using Server.model;

namespace Server
{
    /// <summary>
    /// Server Class
    /// </summary>
    public class Server
    {
        private readonly int port;
        private static UserList _allUsers;
        private static readonly UserList _connectedUsers = new UserList();
        private static readonly UsersToTopics _topicList = new UsersToTopics();
        public static readonly Dictionary<User, TcpClient> TcpClients = new Dictionary<User, TcpClient>();

        public static UsersToTopics TopicList => _topicList;

        public static UserList AllUsers => _allUsers;
        public static UserList ConnectedUsers => _connectedUsers;


        public Server(int port)
        {
            this.port = port;
            _allUsers = new UserList(@"..\..\..\savedFiles\users.bin");
            _allUsers.LoadUsers();
        }

        /// <summary>
        /// Accept new connections to the server
        /// </summary>
        public void Start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] {127, 0, 0, 1}), port);
            l.Start();

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm);
                new Thread(new Receiver(comm).Listener).Start();
            }
        }
    }
}
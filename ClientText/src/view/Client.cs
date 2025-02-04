﻿using System;
using System.Net.Sockets;
using System.Threading;
using ClientText.controller;
using Communication.model;

namespace ClientText.view
{
    /// <summary>
    /// Main view class
    /// </summary>
    public partial class Client
    {
        public static User CurrentUser;
        public static TcpClient Connection;
        private string hostname;
        private int port;

        public Client(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
        }

        public void Start()
        {
            if (Connect(hostname, port))
            {
                Console.Out.WriteLine("Connection established");
                MainLoop();
                Close();
            }
            else
            {
                Console.Out.WriteLine("Connection with the server failed");
            }
        }

        /// <summary>
        /// Start the connection to the server
        /// </summary>
        /// <param name="hostname">Server address</param>
        /// <param name="port">Server port</param>
        /// <returns>Integrity of operation</returns>
        public static bool Connect(string hostname, int port)
        {
            try
            {
                Connection = new TcpClient(hostname, port);
                return Connection != null;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public static void Close()
        {
            CurrentUser = null;
            Connection.Close();
        }

        /// <summary>
        /// Main loop ruling user interaction
        /// </summary>
        private void MainLoop()
        {
            EntryLoop();
            if (CurrentUser == null) return;

            Thread t = new Thread(new Listener().Loop);
            t.Start();

            Console.Clear();
            ChatInput();
        }
    }
}
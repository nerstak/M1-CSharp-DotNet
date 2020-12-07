using System;
using System.Collections.Generic;
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
        private string hostname;
        private int port;

        public Client(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
        }

        public void Start()
        {
            AbstractAction.Start(hostname,port);
            Console.Out.WriteLine("Connection established");
            MainLoop();
        }
        
        /// <summary>
        /// Main loop ruling user interaction
        /// </summary>
        private void MainLoop()
        {
            EntryLoop();
            if (CurrentUser != null)
            {
                // MainMenu.Loop();
            }
        }
    }
}
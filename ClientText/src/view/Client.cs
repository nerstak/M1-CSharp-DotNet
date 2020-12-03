using System;
using ClientText.controller;

namespace ClientText.view
{
    public class Client
    {
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

        private void MainLoop()
        {
            UserConnection();
        }

        private void UserConnection()
        {
            bool flag = false;
            do
            {
                Console.Out.WriteLine("Username: ");
                var username = Console.ReadLine();
                Console.Out.WriteLine("Password: ");
                var password = Console.ReadLine();

                flag = Login.ConnectUser(username, password);
                if (!flag)
                {
                    Console.Out.WriteLine("Wrong credentials");
                }
            } while (!flag);
            Console.Out.WriteLine("You have been connected!");
        }
    }
}
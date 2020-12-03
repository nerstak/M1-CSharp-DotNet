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
            UserAction userAction = new UserAction();
            do
            {
                Console.Out.WriteLine("Username: ");
                var username = Console.ReadLine();
                Console.Out.WriteLine("Password: ");
                var password = Console.ReadLine();

                string message;
                flag = userAction.ConnectUser(username, password, out message);
                if (!flag)
                {
                    Console.Out.WriteLine(message);
                }
            } while (!flag);
            Console.Out.WriteLine("You have been connected!");
        }

        private void UserCreation()
        {
            bool flag = false;
            UserAction userAction = new UserAction();
            do
            {
                Console.Out.WriteLine("Username: ");
                var username = Console.ReadLine();
                Console.Out.WriteLine("Password: ");
                var password = Console.ReadLine();

                flag = userAction.CreateUser(username, password, out var message);
                if (!flag)
                {
                    Console.Out.WriteLine(message);
                }
            } while (!flag);
            Console.Out.WriteLine("You have been connected!");
            
        }
    }
}
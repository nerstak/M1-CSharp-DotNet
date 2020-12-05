using System;
using ClientText.controller;
using Communication.model;

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
            //UserCreation();
            UserConnection();
            TopicCreation();
            TopicCreation();
            TopicList();
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

                flag = userAction.ConnectUser(username, password, out var message);
                Console.Out.WriteLine(message);
            } while (!flag);
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
                Console.Out.WriteLine(message);
            } while (!flag);
        }
        
        private void TopicCreation()
        {
            bool flag = false;
            TopicAction topicAction = new TopicAction();
            do
            {
                Console.Out.WriteLine("Topic name: ");
                var name = Console.ReadLine();

                flag = topicAction.CreateTopic(name, out var message);
                Console.Out.WriteLine(message);
            } while (!flag);
        }

        private void TopicList()
        {
            TopicAction topicAction = new TopicAction();
            TopicList topicList = topicAction.ListTopics(out var message);

            if (topicList == null || message != null)
            {
                Console.Out.WriteLine(message);
            }
            else
            {
                foreach (var topic in topicList.List)
                {
                    Console.Out.WriteLine("   " + topic.Name);
                }
            }
        }
    }
}
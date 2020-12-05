using System;
using ClientText.controller;
using Communication.model;

namespace ClientText.view
{
    /// <summary>
    /// Main Menu
    /// </summary>
    public class MainMenu
    {
        /// <summary>
        /// Loop for logged out user
        /// </summary>
        public static void Loop()
        {
            bool leave = false;
            do
            {
                int value = 0;
                try
                {
                    Console.Out.Write("1. Join topic \n2. List of topics \n3. Create topic \n4. Private messages\n5. Leave \n");
                    value = Int32.Parse(Console.ReadLine());
                    switch (value)
                    {
                        case 1:
                            // Action
                            break;
                        case 2:
                            TopicList();
                            break;
                        case 3:
                            TopicCreation();
                            break;
                        case 4:
                            // Action
                            break;
                        case 5:
                            leave = true;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Incorrect input");
                }
                

            } while (leave == false);
        }
        
        /// <summary>
        /// Links between interface and controller for Topic Listing
        /// </summary>
        private static void TopicList()
        {
            TopicAction topicAction = new TopicAction();
            TopicList topicList = topicAction.ListTopics(out var message);

            if (topicList == null || message != null)
            {
                Console.Out.WriteLine(message);
            }
            else
            {
                Console.Out.WriteLine("List of all topics: ");
                foreach (var topic in topicList.List)
                {
                    Console.Out.WriteLine("   " + topic.Name);
                }
            }
        }
        
        /// <summary>
        /// Links between interface and controller for Topic Creation
        /// </summary>
        private static void TopicCreation()
        {
            bool flag = false;
            TopicAction topicAction = new TopicAction();
            do
            {
                Console.Out.WriteLine("Creation of topic");
                Console.Out.WriteLine("Topic name: ");
                var name = Console.ReadLine();

                flag = topicAction.CreateTopic(name, out var message);
                Console.Out.WriteLine(message);
            } while (!flag);
        }
    }
}
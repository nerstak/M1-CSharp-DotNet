using System;
using ClientText.controller;

namespace ClientText.view
{
    public class Logout
    {
        public static void Loop()
        {
            bool leave = false;
            do
            {
                int value = 0;
                try
                {
                    Console.Out.Write("1. Create User \n2. Log in \n3. Leave \n");
                    value = Int32.Parse(Console.ReadLine());
                    switch (value)
                    {
                        case 1:
                            UserInput("Create your user", UserAction.CreateUserPacket);
                            break;
                        case 2:
                            UserInput("Login", UserAction.LoginUserPacket);
                            Console.Out.WriteLine();
                            break;
                        case 3:
                            leave = true;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Incorrect input");
                }
                

            } while (leave == false && Client.User == null);
        }
        
        /// <summary>
        /// Links between interface and controller for User Input when logged out
        /// </summary>
        private static void UserInput(string informationMessage, UserAction.PacketCreation packetCreation)
        {
            bool flag = false;
            UserAction userAction = new UserAction();
            do
            {
                Console.Out.WriteLine(informationMessage);
                
                Console.Out.WriteLine("Username (write \"leave\" to leave): ");
                var username = Console.ReadLine();
                
                // Checking if user wants to leave
                if (username != null && username.Equals("leave"))
                {
                    flag = true;
                }
                else
                {
                    Console.Out.WriteLine("Password: ");
                    var password = Console.ReadLine();

                    flag = userAction.HandleUser(username, password, out var message, packetCreation);
                    Console.Out.WriteLine(message);
                }
            } while (!flag);
        }
    }
}
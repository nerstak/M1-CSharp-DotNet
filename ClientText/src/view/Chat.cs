using System;
using ClientText.controller;
using Communication.model;
using Communication.utils;

namespace ClientText.view
{
    public partial class Client
    {
        /// <summary>
        /// Handle chat inputs
        /// </summary>
        private void ChatInput()
        {
            // No more current user means that a logout happened
            while (CurrentUser != null)
            {
                CommandParser commandParser = new CommandParser();
                CustomPacket customPacket = commandParser.ParseCommand(Console.In.ReadLine());
                
                if (customPacket == null)
                {
                    // Displaying errors
                    Console.Out.WriteLine(commandParser.Message);
                }
                else
                {
                    // Sending
                    Send(customPacket);
                }
            }
        }

        /// <summary>
        /// Send data to server
        /// </summary>
        /// <param name="customPacket">Packet</param>
        private void Send(CustomPacket customPacket)
        {
            try
            {
                // Sending
                Net.sendMsg(Connection.GetStream(), customPacket);
            }
            catch (Exception)
            {
                // Fail
                if (Connection != null)
                {
                    Console.Out.WriteLine("Connection error");
                }
            }
        }
    }
}
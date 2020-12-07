using System;
using ClientText.view;
using Communication.model;
using Communication.utils;

namespace ClientText.controller
{
    public class Listener
    {
        /// <summary>
        /// Loop handling listener
        /// </summary>
        public void Loop()
        {
            while (Client.CurrentUser != null)
            {
                try
                {
                    CustomPacket customPacket = Net.rcvMsg(Client.Connection.GetStream());
                    if(customPacket == null) continue;
                    Console.Out.WriteLine(customPacket.ToString());
                }
                catch (Exception e)
                {
                    if (Client.Connection != null)
                    {
                        Console.Out.WriteLine("Connection error");
                    }
                }
            }
        }
    }
}
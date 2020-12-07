using System;
using System.Net.Sockets;
using System.Threading;
using ClientText.view;
using Communication.model;
using Communication.utils;

namespace ClientText.controller
{
    public class Listener
    {
        private int _connectTry = 0;
        /// <summary>
        /// Loop handling listener
        /// </summary>
        public void Loop()
        {
            while (Client.CurrentUser != null && Client.Connection != null)
            {
                try
                {
                    CustomPacket customPacket = Net.rcvMsg(Client.Connection.GetStream());
                    if(customPacket == null) continue;
                    Console.Out.WriteLine(customPacket.ToString());

                    _connectTry = 0;
                }
                catch (Exception e)
                {
                    if (Client.Connection != null)
                    {
                        if (Client.CurrentUser != null)
                        {
                            Console.Out.WriteLine("Connection error");
                        }
                        
                        Thread.Sleep(1000);

                        if (_connectTry == 5)
                        {
                            Client.Close();
                        } 
                        _connectTry++;
                    }
                }
            }
        }
    }
}
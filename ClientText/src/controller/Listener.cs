using System;
using System.Threading;
using ClientText.view;
using Communication.model;
using Communication.utils;

namespace ClientText.controller
{
    // Class listening to incoming packets from server
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
                    // Listening
                    CustomPacket customPacket = Net.rcvMsg(Client.Connection.GetStream());
                    if(customPacket == null) continue;
                    Console.Out.WriteLine(customPacket.ToString());

                    _connectTry = 0;
                }
                catch (Exception)
                {
                    // Fail
                    
                    // If the client connection is null, it means that there has been a logout
                    if (Client.Connection != null)
                    {
                        if (Client.CurrentUser != null)
                        {
                            Console.Out.WriteLine("Connection error");
                        }
                        
                        Timeout();
                    }
                }
            }
        }

        /// <summary>
        /// Function responsible for the timeout (letting it go)
        /// </summary>
        private void Timeout()
        {
            Thread.Sleep(1500);
            if (_connectTry == 5)
            {
                Client.Close();
            } 
            _connectTry++;
        }
    }
}
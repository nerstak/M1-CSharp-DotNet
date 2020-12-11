using System;
using System.Threading;
using System.Windows;
using Communication.model;
using Communication.utils;

namespace ClientGUI
{
    public partial class ChatWindow: Window
    {
        private int _connectTry = 0;
        
        /// <summary>
        /// Loop handling listener
        /// </summary>
        public void Loop()
        {
            while (_user != null && Connection != null)
            {
                try
                {
                    // Listening
                    CustomPacket customPacket = Net.rcvMsg(Connection.GetStream());
                    AddLineChat(customPacket.ToString());

                    _connectTry = 0;
                }
                catch (Exception)
                {
                    // Fail
                    
                    // If the client connection is null, it means that there has been a logout
                    if (Connection != null)
                    {
                        if (_user != null)
                        {
                            AddLineChat("Connection error");
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
                Connection.Close();
            } 
            _connectTry++;
        }
    }
}
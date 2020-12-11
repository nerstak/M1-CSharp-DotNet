using System;
using System.Net.Sockets;
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
        private void Loop()
        {
            while (CurrentUser != null && Connection != null)
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
                        if (CurrentUser != null)
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
                Connection = null;
                MessageBox.Show("Connection with the server failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            _connectTry++;
        }
    }
}
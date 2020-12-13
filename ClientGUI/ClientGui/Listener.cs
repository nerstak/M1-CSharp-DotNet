using System;
using System.Threading;
using System.Windows;
using Communication.model;
using Communication.utils;

namespace ClientGUI
{
    // Handle Listener
    public partial class ChatWindow : Window
    {
        private int _connectTry = 0;

        /// <summary>
        /// Loop handling listener
        /// </summary>
        private void Loop()
        {
            while (CurrentUser != null && _connection != null)
            {
                try
                {
                    // Listening
                    CustomPacket customPacket = Net.rcvMsg(_connection.GetStream());
                    AddLineChat(customPacket.ToString());

                    _connectTry = 0;
                }
                catch (Exception)
                {
                    // Fail

                    // If the client connection is null, it means that there has been a logout
                    if (_connection != null)
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
                // We stop the application after a few seconds of errors
                _connection.Close();
                _connection = null;
                MessageBox.Show("Connection with the server failed", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            _connectTry++;
        }
    }
}
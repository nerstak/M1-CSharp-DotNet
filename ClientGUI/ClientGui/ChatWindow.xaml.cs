using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ClientGui;
using Communication.model;
using Communication.utils;

namespace ClientGUI
{
    public partial class ChatWindow : Window
    {
        public static User CurrentUser = null;
        public static TcpClient Connection = null;

        public ChatWindow(User currentUser, TcpClient tcpClient)
        {
            CurrentUser = currentUser;
            Connection = tcpClient;
            InitializeComponent();
            ChatTextBlock.Text = "";
            new Thread(Loop).Start();
        }

        /// <summary>
        /// Write a new line
        /// </summary>
        /// <param name="line">Line to write</param>
        private void AddLineChat(string line)
        {
            this.Dispatcher.Invoke(() =>
                ChatTextBlock.Text = ChatTextBlock.Text + "\n" + line
            );
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Connection.Close();
            CurrentUser = null;
            base.OnClosing(e);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                HandleInput();
            }
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            HandleInput();
        }

        /// <summary>
        /// Handle chat inputs
        /// </summary>
        private void HandleInput()
        {
            CommandParser commandParser = new CommandParser();
            CustomPacket customPacket = commandParser.ParseCommand(InputBox.Text);
                
            if (customPacket == null)
            {
                // Displaying errors
                AddLineChat(commandParser.Message);
            }
            else
            {
                // Sending
                Send(customPacket);
            }

            InputBox.Text = "";
            if (CurrentUser == null)
            {
                this.Close();
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
                    MessageBox.Show("Connection with the server failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
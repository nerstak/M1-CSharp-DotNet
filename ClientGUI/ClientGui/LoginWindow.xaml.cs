﻿using System.Net.Sockets;
using System.Windows;
using ClientGUI;
using Communication.model;

namespace ClientGui
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private User _user = null;
        public static TcpClient Connection = null;
        
        private string hostname = "127.0.0.1";
        private int port = 8976;


        public LoginWindow()
        {
            InitializeComponent();
            Start();
            _user = new User();
        }

        private void CreateAccountButton_OnClick(object sender, RoutedEventArgs e)
        {
            FillUser();
            if (new MenuActions().HandleUser(_user, out var msg, MenuActions.CreateUserPacket))
            {
                MessageBox.Show(msg, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            FillUser();
            if (new MenuActions().HandleUser(_user, out var msg, MenuActions.LoginUserPacket))
            {
                // Load window
                new ChatWindow(_user,Connection).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Fill user attribute from TextBoxes
        /// </summary>
        private void FillUser()
        {
            _user.Username = UsernameText.Text;
            _user.Password = PasswordText.Text;
        }
        
        /// <summary>
        /// Handle starting connection
        /// </summary>
        private void Start()
        {
            if (!Connect())
            {
                MessageBox.Show("Connection with the server failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Start the connection to the server
        /// </summary>
        /// <returns>Integrity of operation</returns>
        private bool Connect()
        {
            try
            {
                Connection = new TcpClient(hostname, port);
                return Connection != null;
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
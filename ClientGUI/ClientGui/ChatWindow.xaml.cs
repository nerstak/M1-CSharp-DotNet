using System.Net.Sockets;
using System.Windows;
using Communication.model;

namespace ClientGUI
{
    public partial class ChatWindow : Window
    {
        private User _user = null;
        public static TcpClient Connection = null;

        public ChatWindow(User user, TcpClient tcpClient)
        {
            _user = user;
            Connection = tcpClient;
            InitializeComponent();
            ChatTextBlock.Text = "";
        }
    }
}
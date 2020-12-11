using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
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
            _user = null;
            base.OnClosing(e);
        }
    }
}
using System.Net.Sockets;
using Communication.model;

namespace ClientText.controller
{
    public abstract class AbstractAction
    {
        protected static TcpClient connection;
        protected static User user;
        protected CustomPacket customPacket;

        public static bool Start(string hostname, int port)
        {
            connection = new TcpClient(hostname,port);
            return connection != null;
        }

        protected string GetInformationMessage()
        {
            return ((InformationMessage) customPacket.Data).Content;
        }
    }
}
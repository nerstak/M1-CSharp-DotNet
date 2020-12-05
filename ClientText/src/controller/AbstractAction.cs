using System.Net.Sockets;
using Communication.model;

namespace ClientText.controller
{
    public abstract class AbstractAction
    {
        protected static TcpClient Connection;
        protected CustomPacket CustomPacket;

        /// <summary>
        /// Start the connection to the server
        /// </summary>
        /// <param name="hostname">Server address</param>
        /// <param name="port">Server port</param>
        /// <returns>Integrity of operation</returns>
        public static bool Start(string hostname, int port)
        {
            Connection = new TcpClient(hostname,port);
            return Connection != null;
        }


        /// <summary>
        /// Get information message data
        /// </summary>
        /// <returns>Data contained, or null</returns>
        protected string GetInformationMessage()
        {
            if (CustomPacket.Data is InformationMessage)
            {
                return ((InformationMessage) CustomPacket.Data).Content;
            }

            return null;
        }
    }
}
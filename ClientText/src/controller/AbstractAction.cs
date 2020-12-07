using System.Net.Sockets;
using Communication.model;

namespace ClientText.controller
{
    public abstract class AbstractAction
    {
        protected CustomPacket CustomPacket;
        

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
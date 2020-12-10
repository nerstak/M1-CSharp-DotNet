using System;
using Communication.utils;
using Communication.model;

namespace ClientGui
{
    /// <summary>
    /// Actions relative to user
    /// </summary>
    public class MenuActions
    {
        private CustomPacket _customPacket;
        

        /// <summary>
        /// Get information message data
        /// </summary>
        /// <returns>Data contained, or null</returns>
        private string GetInformationMessage()
        {
            if (_customPacket.Data is InformationMessage)
            {
                return ((InformationMessage) _customPacket.Data).Content;
            }

            return null;
        }
        public delegate CustomPacket PacketCreation(User user);

        /// <summary>
        /// Handle an user
        /// </summary>
        /// <param name="u">User</param>
        /// <param name="message">Information message</param>
        /// <param name="packetCreation">Function to handle packet creation</param>
        /// <returns>Validity of operation</returns>
        public bool HandleUser(User u, out string message, PacketCreation packetCreation)
        {
            message = null;

            _customPacket = packetCreation(u);
            try
            {
                Net.sendMsg(LoginWindow.Connection.GetStream(), _customPacket);

                _customPacket = Net.rcvMsg(LoginWindow.Connection.GetStream());
                message = GetInformationMessage();
                if (_customPacket.Operation == Operation.Reception)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                message = "Connection failed";
                Console.Out.WriteLine("Error: " + e);
            }
            
            return false;
        }

        public static CustomPacket CreateUserPacket(User u)
        {
            return new CustomPacket(Operation.CreateUser, u);
        }
        
        public static CustomPacket LoginUserPacket(User u)
        {
            return new CustomPacket(Operation.LoginUser, u);
        }
    }
}
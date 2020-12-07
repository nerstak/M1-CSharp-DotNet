using System;
using ClientText.view;
using Communication.utils;
using Communication.model;

namespace ClientText.controller
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
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="message">Information message</param>
        /// <param name="packetCreation">Function to handle packet creation</param>
        /// <returns>Validity of operation</returns>
        public bool HandleUser(string username, string password, out string message, PacketCreation packetCreation)
        {
            message = null;
            var u = new User {Username = username, Password = password};

            _customPacket = packetCreation(u);
            try
            {
                Net.sendMsg(Client.Connection.GetStream(), _customPacket);

                _customPacket = Net.rcvMsg(Client.Connection.GetStream());
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
            
            // We set User to null, because the LoginUserPacket() set it to the value of the user
            Client.CurrentUser = null;
            
            return false;
        }

        public static CustomPacket CreateUserPacket(User u)
        {
            return new CustomPacket(Operation.CreateUser, u);
        }
        
        public static CustomPacket LoginUserPacket(User u)
        {
            Client.CurrentUser = u;
            return new CustomPacket(Operation.LoginUser, u);
        }
    }
}
using System;
using Communication.utils;
using Communication.model;

namespace ClientText.controller
{
    /// <summary>
    /// Actions relative to user
    /// </summary>
    public class UserAction: AbstractAction
    {
        /// <summary>
        /// Connect an user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="message">Information message</param>
        /// <returns>Validity of operation</returns>
        public bool ConnectUser(string username, string password, out string message)
        {
            message = null;
            var u = new User {Username = username, Password = password};

            CustomPacket = new CustomPacket(Operation.LoginUser, u);
            try
            {
                Net.sendMsg(Connection.GetStream(), CustomPacket);

                CustomPacket = Net.rcvMsg(Connection.GetStream());
                message = GetInformationMessage();
                if (CustomPacket.OperationOrder == Operation.Reception)
                {
                    User = u;
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error: " + e);
            }
            
            return false;
        }

        /// <summary>
        /// Create an user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="message">Information message</param>
        /// <returns>Validity of operation</returns>
        public bool CreateUser(string username, string password, out string message)
        {
            message = null;
            var u = new User {Username = username, Password = password};

            CustomPacket = new CustomPacket(Operation.CreateUser, u);
            try
            {
                Net.sendMsg(Connection.GetStream(), CustomPacket);

                CustomPacket = Net.rcvMsg(Connection.GetStream());
                message = GetInformationMessage();
                if (CustomPacket.OperationOrder == Operation.Reception)
                {
                    User = u;
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
    }
}
using System;
using Communication;
using Communication.model;

namespace ClientText.controller
{
    public class Login: AbstractAction
    {
        public static bool ConnectUser(string username, string password)
        {
            User u = new User();
            u.Username = username;
            u.Password = password;
            
            CustomPacket customPacket = new CustomPacket(Operation.LoginUser, u);
            try
            {
                Net.sendMsg(connection.GetStream(), customPacket);

                customPacket = Net.rcvMsg(connection.GetStream());
                if (customPacket.OperationOrder == Operation.Reception)
                {
                    user = u;
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error: " + e);
            }
            

            return false;
        }
    }
}
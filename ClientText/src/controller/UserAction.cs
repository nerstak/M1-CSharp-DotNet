using System;
using Communication.utils;
using Communication.model;

namespace ClientText.controller
{
    public class UserAction: AbstractAction
    {
        public bool ConnectUser(string username, string password, out string message)
        {
            message = null;
            var u = new User {Username = username, Password = password};

            customPacket = new CustomPacket(Operation.LoginUser, u);
            try
            {
                Net.sendMsg(connection.GetStream(), customPacket);

                customPacket = Net.rcvMsg(connection.GetStream());
                message = GetInformationMessage();
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

        public bool CreateUser(string username, string password, out string message)
        {
            message = null;
            var u = new User {Username = username, Password = password};

            customPacket = new CustomPacket(Operation.CreateUser, u);
            try
            {
                Net.sendMsg(connection.GetStream(), customPacket);

                customPacket = Net.rcvMsg(connection.GetStream());
                message = GetInformationMessage();
                if (customPacket.OperationOrder == Operation.Reception)
                {
                    user = u;
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

        public TopicList listTopics(out string message)
        {
            message = null;
            customPacket = new CustomPacket(Operation.ListTopics, null);
            try
            {
                Net.sendMsg(connection.GetStream(), customPacket);

                customPacket = Net.rcvMsg(connection.GetStream());
                
                if (customPacket.OperationOrder == Operation.Reception && (customPacket.Data) is TopicList list)
                {
                    return (TopicList) list;
                }
                throw new Exception("Wrong type given");
            }
            catch (Exception e)
            {
                message = "Connection failed";
                Console.Out.WriteLine("Error: " + e);
            }

            return null;
        }
    }
}
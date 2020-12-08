using Communication.model;

namespace Server
{
    public partial class Receiver
    {
        /// <summary>
        /// Create an user
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket CreateUser(CustomPacket customPacket)
        {
            var u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            
            // We add the user if it does not already exists
            if (Server.AllUsers.SearchUser(u) == null)
            {
                Server.AllUsers.AddUser(u);
                Server.AllUsers.Semaphore.Release();
                return new CustomPacket(Operation.Reception, new InformationMessage("Account created"));
            }
            else
            {
                Server.AllUsers.Semaphore.Release();
                return
                    new CustomPacket(Operation.Refused, new InformationMessage("User already existing"));
            }
        }

        /// <summary>
        /// Log an user
        /// </summary>
        /// <param name="customPacket">Packet received</param>
        /// <returns>Packet to send</returns>
        private CustomPacket LoginUser(CustomPacket customPacket)
        {
            User u = (User) customPacket.Data;
            Server.AllUsers.Semaphore.WaitOne();
            var searchedUser = Server.AllUsers.SearchUser(u);
            Server.AllUsers.Semaphore.Release();

            string errorMessage = "";
            if (searchedUser != null) // No need to check for credentials, SearchUser already did it
            {
                if (Server.ConnectedUsers.SearchUser(searchedUser) == null) // No double connection
                {
                    Server.ConnectedUsers.Semaphore.WaitOne();
                    Server.ConnectedUsers.AddUser(u);
                    _user = u;
                    Server.TcpClients.Add(_user,comm);
                    Server.ConnectedUsers.Semaphore.Release();
                    Connecting();
                    return new CustomPacket(Operation.Reception, new InformationMessage("Connected"));
                }

                errorMessage = "You are already connected";
            }
            else
            {
                errorMessage = "Wrong credentials";
            }
            
            
            return new CustomPacket(Operation.Refused, new InformationMessage(errorMessage));
        }
    }
}
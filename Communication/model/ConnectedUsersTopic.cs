using System;
using System.Collections.Generic;

namespace Communication.model
{
    public class ConnectedUsersTopic
    {
        private Topic topic;
        private List<User> users;

        public ConnectedUsersTopic(Topic topic)
        {
            this.topic = topic;
            users = new List<User>();
        }

        public Topic Topic
        {
            get => topic;
        }

        public User SearchUser(String username)
        {
            return users.Find(u => u.Username.Equals(username));
        }

        public void AddUser(User u)
        {
            if (!users.Contains(u))
            {
                users.Add(u);
            }
        }

        public void RemoveUser(User u)
        {
            users.Remove(u);
        }

        public void RemoveUser(String username)
        {
            users.Remove(SearchUser(username));
        }
    }
}
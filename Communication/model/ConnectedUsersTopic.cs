using System;
using System.Collections.Generic;

namespace Communication.model
{
    public class ConnectedUsersTopic
    {
        private Topic topic;
        private List<User> users;
        private Queue<Message> history;

        public ConnectedUsersTopic(Topic topic)
        {
            this.topic = topic;
            users = new List<User>();
            history = new Queue<Message>();
        }

        public Topic Topic
        {
            get => topic;
        }

        public User SearchUser(String username)
        {
            return users.Find(u => u.Username.Equals(username));
        }

        public User SearchUser(User user)
        {
            User u = SearchUser(user.Username);
            if (u != null)
            {
                if (u.CheckPassword(user.Password))
                {
                    return u;
                }
            }

            return null;
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

        public void SaveUsers(string file)
        {
            FileStreamer<User>.BinSave(users, file);
        }
        
        public void LoadUsers(string file)
        {
            users = FileStreamer<User>.BinLoad(file);
        }
    }
}
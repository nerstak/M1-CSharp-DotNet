using System;
using System.Collections.Generic;
using System.Threading;
using Communication.utils;

namespace Communication.model
{
    public class UserList
    {
        private List<User> _users;
        private readonly Semaphore _semaphore = new Semaphore(1,1);
        private readonly string _filePath = string.Empty;
        
        public UserList()
        {
            _users = new List<User>();
        }

        public UserList(string filePath)
        {
            _filePath = filePath;
            _users = new List<User>();
        }

        public List<User> Users => _users;
        public Semaphore Semaphore => _semaphore;

        /// <summary>
        /// Search an user in the list
        /// </summary>
        /// <param name="username">User to search</param>
        /// <returns>User, may be null</returns>
        public User SearchUsername(string username)
        {
            return _users?.Find(u => username.Equals(u.Username));
        }

        /// <summary>
        /// Search an identical user in the list
        /// </summary>
        /// <param name="user">User to search</param>
        /// <returns>User, may be null</returns>
        public User SearchUser(User user)
        {
            return _users?.Find(u => u.Equals(user));
        }

        /// <summary>
        /// Add an user
        /// Automatically save the list if a file is linked
        /// </summary>
        /// <param name="u">User to add</param>
        public void AddUser(User u)
        {
            if (!_users.Contains(u))
            {
                _users.Add(u);
                
                // Auto saving to file
                if (!string.IsNullOrWhiteSpace(_filePath))
                {
                    SaveUsers(_filePath);
                }
            }
        }

        /// <summary>
        /// Remove an user
        /// </summary>
        /// <param name="u">User to remove</param>
        public void RemoveUser(User u)
        {
            _users.Remove(u);
        }

        /// <summary>
        /// Remove an user
        /// </summary>
        /// <param name="username">User to remove</param>
        public void RemoveUser(string username)
        {
            _users.Remove(SearchUsername(username));
        }

        /// <summary>
        /// Save a  list of users
        /// </summary>
        /// <param name="file">File to save to</param>
        public void SaveUsers(string file)
        {
            FileStreamer<User>.BinSave(_users, file);
        }
        
        /// <summary>
        /// Load list of user from file
        /// </summary>
        /// <param name="file">path to file</param>
        public void LoadUsers(string file)
        {
            try
            {
                List<User> tmp = FileStreamer<User>.BinLoad(file);
                if (tmp != null)
                {
                    _users = tmp;
                }
            }
            catch (Exception e)
            {
                FileStreamer<User>.CreateFile(file);
                Console.Out.WriteLine(e);
            }
            
        }

        /// <summary>
        /// Load list of user from file associated with list
        /// </summary>
        public void LoadUsers()
        {
            if (!string.IsNullOrWhiteSpace(_filePath))
            {
                LoadUsers(_filePath);
            }
        }
    }
}
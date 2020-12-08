using System.Collections.Generic;
using System.Threading;
using Communication.model;

namespace Server.model
{
    /// <summary>
    /// List all topics, and users connected to themo
    /// </summary>
    public class UsersToTopics
    {
        private readonly Semaphore _semaphore = new Semaphore(1,1);
        private readonly Dictionary<Topic, UserList> _list = new Dictionary<Topic, UserList>();

        public Semaphore Semaphore => _semaphore;

        public Dictionary<Topic, UserList> List => _list;

        /// <summary>
        /// Search for a Topic
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <returns>Userlist or null</returns>
        public UserList SearchTopic(Topic topic)
        {
            if (_list.ContainsKey(topic))
            {
                return _list[topic];
            }

            return null;
        }

        /// <summary>
        /// Get list of all topics
        /// </summary>
        /// <returns>List of topic</returns>
        public TopicList GetListTopics()
        {
            _semaphore.WaitOne();
            var topics = new TopicList();
            foreach (var cut in _list)
            {
                topics.List.Add(cut.Key);
            }

            _semaphore.Release();
            return topics;
        }

        /// <summary>
        /// Check if an user is connected to a specific topic
        /// </summary>
        /// <param name="t">Topic</param>
        /// <param name="u">User</param>
        /// <returns>Boolean</returns>
        public bool CheckUserConnectionTopic(Topic t, User u)
        {
            var connectTopic = SearchTopic(t);

            if(connectTopic?.SearchUsername(u.Username) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove an user from all topics
        /// </summary>
        /// <param name="u">User to remove</param>
        public void RemoveUserFromAll(User u)
        {
            foreach (var topic in _list)
            {
                topic.Value.RemoveUser(u);
            }
        }
    }
}
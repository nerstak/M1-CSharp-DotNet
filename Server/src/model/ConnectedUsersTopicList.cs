using System.Collections.Generic;
using System.Threading;
using Communication.model;

namespace Server.model
{
    public class ConnectedUsersTopicList
    {
        private readonly Semaphore _semaphore = new Semaphore(1,1);
        private readonly List<ConnectedUsersTopic> _list = new List<ConnectedUsersTopic>();

        public Semaphore Semaphore => _semaphore;

        public List<ConnectedUsersTopic> List => _list;

        /// <summary>
        /// Search for a Topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public ConnectedUsersTopic SearchTopic(Topic topic)
        {
            return _list.Find(t => t.Topic.Equals(topic));
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
                topics.List.Add(cut.Topic);
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

            if (connectTopic?.UserList.SearchUsername(u.Username) != null)
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
                topic.UserList.RemoveUser(u);
            }
        }
    }
}
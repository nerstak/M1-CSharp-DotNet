using System.Collections.Generic;
using System.Threading;
using Communication.model;

namespace Server.model
{
    public class ConnectedUsersTopicList
    {
        private readonly Semaphore _semaphore = new Semaphore(1,1);
        private List<ConnectedUsersTopic> _list = new List<ConnectedUsersTopic>();

        public Semaphore Semaphore => _semaphore;

        public List<ConnectedUsersTopic> List => _list;

        public ConnectedUsersTopic SearchTopic(Topic topic)
        {
            return _list.Find(t => t.Topic.Equals(topic));
        }

        /// <summary>
        /// Get list of all topics
        /// </summary>
        /// <param name="cutList">List of Connected Users Topic</param>
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

        public bool CheckUserConnectionTopic(Topic t, User u)
        {
            var connectTopic = _list.Find(tp => tp.Topic.Equals(t));

            if (connectTopic?.UserList.SearchUser(u) != null)
            {
                return true;
            }

            return false;
        }
    }
}
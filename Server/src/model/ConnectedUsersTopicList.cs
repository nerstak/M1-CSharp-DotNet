using System.Collections.Generic;
using System.Threading;
using Communication.model;

namespace Server.model
{
    public class ConnectedUsersTopicList
    {
        private readonly Semaphore _semaphore = new Semaphore(1,1);
        private List<ConnectedUsersTopic> _connectedUsersTopics = new List<ConnectedUsersTopic>();

        public Semaphore Semaphore => _semaphore;

        public List<ConnectedUsersTopic> ConnectedUsersTopics => _connectedUsersTopics;

        public ConnectedUsersTopic SearchTopic(Topic topic)
        {
            return _connectedUsersTopics.Find(t => t.Topic.Equals(topic));
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
            foreach (var cut in _connectedUsersTopics)
            {
                topics.List.Add(cut.Topic);
            }

            _semaphore.Release();
            return topics;
        }

    }
}
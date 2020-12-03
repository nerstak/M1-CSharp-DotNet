using System;
using System.Collections.Generic;
using System.Threading;

namespace Communication.model
{
    public class ConnectedUsersTopic
    {
        private Topic _topic;
        private UserList _userList;
        
        private Queue<Message> _history;
        private Semaphore _semaphoreHistory;

        public ConnectedUsersTopic(Topic topic)
        {
            this._topic = topic;
            _userList = new UserList();
            _history = new Queue<Message>();
        }

        public Topic Topic
        {
            get => _topic;
        }

        public UserList UserList => _userList;
    }
}
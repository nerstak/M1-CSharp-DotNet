using System;
using System.Collections.Generic;
using System.Threading;

namespace Communication.model
{
    public class ConnectedUsersTopic
    {
        private Topic _topic;
        private UserList _userList;

        public ConnectedUsersTopic(Topic topic)
        {
            _topic = topic;
            _userList = new UserList();
        }

        public Topic Topic
        {
            get => _topic;
        }

        public UserList UserList => _userList;
    }
}
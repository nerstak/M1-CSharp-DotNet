using System;
using System.Collections.Generic;

namespace Communication.model
{
    [Serializable]
    public class TopicList: DataPacket
    {
        private List<Topic> _list = new List<Topic>();

        public List<Topic> List => _list;
    }
}
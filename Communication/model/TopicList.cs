using System.Collections.Generic;

namespace Communication.model
{
    public class TopicList: DataPacket
    {
        private List<Topic> _list;

        public List<Topic> List => _list;
    }
}
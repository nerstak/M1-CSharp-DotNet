using System;
using System.Collections.Generic;
using System.Linq;

namespace Communication.model
{
    [Serializable]
    public class TopicList: DataPacket
    {
        private List<Topic> _list = new List<Topic>();

        public List<Topic> List => _list;

        public override string ToString()
        {
            string tmp = "List of topics\n";
            foreach (var t in _list)
            {
                tmp += "   " + t.Name + "\n";
            }

            return tmp;
        }
    }
}
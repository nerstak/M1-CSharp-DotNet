using System;
using System.Collections.Generic;

namespace Communication.model
{
    /// <summary>
    /// List of topics
    /// </summary>
    [Serializable]
    public class TopicList: IDataPacket
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
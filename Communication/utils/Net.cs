using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication.model;

namespace Communication
{
    
    public class Net
    {
        public static void sendMsg(Stream s, CustomPacket pck)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, pck);
        }

        public static CustomPacket rcvMsg(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (CustomPacket)bf.Deserialize(s);
        }
    }
}

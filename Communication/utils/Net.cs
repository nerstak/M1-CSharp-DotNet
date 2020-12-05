using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication.model;

namespace Communication.utils
{
    
    public class Net
    {
        public static void sendMsg(Stream s, CustomPacket pck)
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(s, pck);
            } catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public static CustomPacket rcvMsg(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                return (CustomPacket) bf.Deserialize(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{
    public static class FileStreamer<T>
    {
        /// <summary>
        /// Save a list in a bin file
        /// </summary>
        /// <param name="list">List to save</param>
        /// <param name="path">Path to file</param>
        public static void BinSave(List<T> list, string path)
        {
            FileStream stream = File.Create(path);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, list);
            stream.Close();
        }

        /// <summary>
        /// Load a list from a bin file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>List loaded</returns>
        public static List<T> BinLoad(string path)
        {
            FileStream stream = File.Open(path,FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            List<T> list = (List<T>) formatter.Deserialize(stream);
            stream.Close();
            return list;
        }
    }
}
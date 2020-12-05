using System.Collections.Generic;

namespace Communication.utils
{
    public class CollectionsMethods<T>
    {
        /// <summary>
        /// Get all keys of a dictionary
        /// </summary>
        /// <param name="dict">Dictionary with keys of type T</param>
        /// <returns>List of keys</returns>
        public static List<T> GetKeys(Dictionary<T, dynamic> dict)
        {
            if (dict == null) return null;
            List<T> keys = new List<T>();
            foreach (var key in dict.Keys)
            {
                keys.Add(key);
            }
            
            return keys;
        }
    }
}
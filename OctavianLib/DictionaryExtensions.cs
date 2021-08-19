using System.Collections.Generic;

namespace OctavianLib
{
    public static class DictionaryExtentions
    {
        public static V GetDefault<T, V>(this Dictionary<T, V> dict, T key, V defaultVal)
        {
            if(dict.ContainsKey(key))
            {
                return dict[key];
            }
            
            return defaultVal;
        }
    }
}
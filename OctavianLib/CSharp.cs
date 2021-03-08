using System.Collections.Generic;

namespace OctavianLib
{
    public static class CSharpExtensions 
    {
        public static Dictionary<TKey, TValue> ReplaceKey<TKey, TValue>(
            this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            var _output = new Dictionary<TKey, TValue>();

            foreach(KeyValuePair<TKey, TValue> _pair in dic) 
            {
                if(EqualityComparer<TKey>.Default.Equals(_pair.Key, fromKey))
                    { _output[toKey] = _pair.Value; } 
                else 
                    { _output[_pair.Key] = _pair.Value; }
            }
            
            return _output;
        }

        public static Dictionary<TKey, TValue> ReplaceVal<TKey, TValue>(
            this IDictionary<TKey, TValue> dic, TValue fromVal, TValue toVal)
        {
            var _output = new Dictionary<TKey, TValue>();

            foreach(KeyValuePair<TKey, TValue> _pair in dic)
            {
                if(EqualityComparer<TValue>.Default.Equals(_pair.Value, fromVal))
                    { _output[_pair.Key] = toVal; } 
                else
                    { _output[_pair.Key] = _pair.Value; }
            }

            return _output;
        }
    }
}
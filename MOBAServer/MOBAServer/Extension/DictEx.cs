using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBAServer.Extension
{
    public static class DictEx
    {
        public static TValue ExTryGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            dict.TryGetValue(key, out value);
            return value;
        }
    }
}

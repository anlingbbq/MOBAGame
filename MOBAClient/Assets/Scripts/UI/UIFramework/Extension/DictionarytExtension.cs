using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionarytExtension {
    /// <summary>
    /// 根据key得到value，没有返回null
    /// </summary>
    public static TValue ExTryGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
    {
        TValue value;
        dict.TryGetValue(key, out value);
        return value;
    }
}

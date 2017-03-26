using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Singlton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            return _Instance;
        }
    }

    protected virtual void Awake()
    {
        _Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        _Instance = null;
    }
}

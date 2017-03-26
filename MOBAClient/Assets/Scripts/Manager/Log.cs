using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 封装Log
/// </summary>
public class Log
{
    public static Action<string> Debug = UnityEngine.Debug.Log;
    public static Action<string> Error = UnityEngine.Debug.LogError;
    public static Action<string> Warning = UnityEngine.Debug.LogWarning;

    // 当发布时 使用空方法
    //public static void Debug(string text) { }
    //public static void Error(string text) { }
    //public static void Warning(string text) { }
}

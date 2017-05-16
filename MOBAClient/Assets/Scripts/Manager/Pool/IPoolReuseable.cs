using System;
using UnityEngine;

public interface IPoolReuseable
{
    /// <summary>
    /// 取出之前的重置操作
    /// </summary>
    void BeforeGetObject();

    /// <summary>
    /// 放入之前的还原操作
    /// </summary>
    void BeforeHideObject();
}

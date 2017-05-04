using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源类
/// </summary>
public class LoadAsset
{
    // 资源信息
    private ResourceRequest Request;

    // 资源名称
    public string AssetName;

    // 资源类型 用于加载资源
    public Type Type;

    // 是否加载完成
    public bool IsDone
    {
        get { return Request != null && Request.isDone; }
    }

    // 获取资源
    public object GetAsset
    {
        get
        {
            if (Request == null)
                return null;
            return Request.asset;
        }
    }

    /// <summary>
    /// 异步加载 
    /// </summary>
    public void LoadAsync()
    {
        Request = Resources.LoadAsync(AssetName, Type);
    }

    // 回掉集合
    public List<IResourceListener> ListenerList;

    /// <summary>
    /// 添加回掉 
    /// </summary>
    /// <param name="listener"></param>
    public void AddListener(IResourceListener listener)
    {
        if (ListenerList == null)
            ListenerList = new List<IResourceListener>();

        if (listener == null)
            return;

        if (ListenerList.Contains(listener))
            return;

        ListenerList.Add(listener);
    }
}

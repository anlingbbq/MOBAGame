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

    // 自定义的资源类型 用于识别
    public AssetType AssetType;

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

    // 异步加载
    public void LoadAsync()
    {
        this.Request = Resources.LoadAsync(AssetName, Type);
    }


    // 回掉集合
    public List<IResourceListener> ListenerList;

    // 添加回掉
    public void AddListener(IResourceListener listener)
    {
        if (ListenerList == null)
        {
            ListenerList = new List<IResourceListener>();
        }

        if (ListenerList.Contains(listener))
        {
            return;
        }

        ListenerList.Add(listener);
    }
}

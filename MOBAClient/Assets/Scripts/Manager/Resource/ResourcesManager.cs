using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理类
/// </summary>
public class ResourcesManager : Singleton<ResourcesManager>
{
    // 已经加载的资源字典
    private Dictionary<string, object> m_AssetDict = new Dictionary<string, object>();

    // 正在加载的资源列表
    private List<LoadAsset> m_LoadingList = new List<LoadAsset>();

    // 等待加载的队列
    private Queue<LoadAsset> m_WaitingQueue = new Queue<LoadAsset>();

    void Update()
    {
        if (m_LoadingList.Count > 0)
        {
            for (int i = 0; i < m_LoadingList.Count; i++)
            {
                if (m_LoadingList[i].IsDone)
                {
                    LoadAsset asset = m_LoadingList[i];
                    for (int j = 0; j < asset.ListenerList.Count; j++)
                    {
                        asset.ListenerList[j].OnLoaded(asset.AssetName, asset.GetAsset, asset.AssetType);
                    }
                    m_AssetDict.Add(asset.AssetName, asset.GetAsset);
                    m_LoadingList.RemoveAt(i);
                }
            }
        }

        while (m_WaitingQueue.Count > 0 && m_LoadingList.Count < 5)
        {
            LoadAsset asset = m_WaitingQueue.Dequeue();
            m_LoadingList.Add(asset);
            asset.LoadAsync();
        }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <param name="listener">回掉</param>
    /// <param name="assetType">自定义的资源类型</param>
    public void Load(string assetName, Type type, IResourceListener listener = null, 
        AssetType assetType = AssetType.Default)
    {
        // 资源已存在 直接完成回掉
        if (m_AssetDict.ContainsKey(assetName))
        {
            if (listener != null)
                listener.OnLoaded(assetName, m_AssetDict[assetName], assetType);
            return;
        }
        // 进行异步加载
        else
        {
            LoadAsync(assetName, type, listener, assetType);
        }
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <param name="listener">回掉</param>
    /// <param name="assetType">自定义的资源类型</param>
    private void LoadAsync(string assetName, Type type, IResourceListener listener, AssetType assetType)
    {
        // 如果需要回调
        if (listener != null)
        {
            // 如果正在加载中 添加回掉
            for (int i = 0; i < m_LoadingList.Count; i++)
            {
                LoadAsset item = m_LoadingList[i];
                if (item.AssetName == assetName)
                {
                    item.AddListener(listener);
                    return;
                }
            }

            // 如果在等待的队列中 添加回掉
            for (int i = 0; i < m_WaitingQueue.Count; i++)
            {
                LoadAsset item = m_WaitingQueue.Peek();
                if (item.AssetName == assetName)
                {
                    item.AddListener(listener);
                    return;
                }
            }
        }
    

        // 都没有 则创建资源
        LoadAsset asset = new LoadAsset();
        asset.AssetName = assetName;
        asset.Type = type;
        asset.AssetType = assetType;
        asset.AddListener(listener);

        // 添加到等待队列
        m_WaitingQueue.Enqueue(asset);
    }

    /// <summary>
    /// 获取资源 
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public object GetAsset(string assetName)
    {
        object asset = null;
        m_AssetDict.TryGetValue(assetName, out asset);
        return asset;
    }

    /// <summary>
    /// 释放资源 
    /// </summary>
    /// <param name="assetName"></param>
    public void ReleaseAsset(string assetName)
    {
        if (m_AssetDict.ContainsKey(assetName))
        {
            m_AssetDict[assetName] = null;
            m_AssetDict.Remove(assetName);
        }
    }

    /// <summary>
    /// 释放所有未使用的资源 
    /// </summary>
    public void ReleaseAll()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}

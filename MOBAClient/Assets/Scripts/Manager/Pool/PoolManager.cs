using UnityEngine;
using System.Collections.Generic;

public class PoolManager
{
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PoolManager();
            }
            return instance;
        }
    }

	// 选择目标路径
    public const string PoolConfigPath = "Assets/Resources/pool.asset";

    // key是池子名字，value是单个池子
    private Dictionary<string, ObjectPool> m_PoolDict = new Dictionary<string, ObjectPool>();

    public PoolManager()
    {
        ObjectPoolList objectPoolList = Resources.Load<ObjectPoolList>("pool");

        foreach (ObjectPool pool in objectPoolList.PoolList)
        {
            this.m_PoolDict.Add(pool.Name, pool);
        }
    }


    /// <summary> 
    /// 根据名字取物体 
    /// </summary>
    public GameObject GetObject(string poolName)
    {
        if (!m_PoolDict.ContainsKey(poolName))
        {
            Debug.LogError("找不到对象池: " + poolName);
            return null;
        }

        ObjectPool pool = m_PoolDict[poolName];
        return pool.GetObject();
    }

    /// <summary> 
    /// 隐藏指定物体 
    /// </summary>
    public void HideObjet(GameObject go)
    {
        foreach (ObjectPool p in m_PoolDict.Values)
        {
            if (p.Contains(go))
            {
                p.HideObject(go);
                return;
            }
        }
    }

    /// <summary> 
    /// 指定一个池子隐藏该池子内的所有物体 
    /// </summary>
    public void HideAllObject(string poolName)
    {
        if (!m_PoolDict.ContainsKey(poolName))
        {
            Debug.LogError("找不到对象池: " + poolName);
            return;
        }
        ObjectPool pool = m_PoolDict[poolName];
        pool.HideAllObject();
    }

    /// <summary> 
    /// 还原对象池 
    /// </summary>
    public void InitAllPool()
    {
        foreach (ObjectPool pool in m_PoolDict.Values)
        {
            pool.InitPool();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 单个池子
/// </summary>
[System.Serializable]
public class ObjectPool
{
    /// <summary> 
    /// 池子的名字 
    /// </summary>
    public string Name;
    /// <summary> 
    /// 池子的对象 
    /// </summary>
    [SerializeField]
    private GameObject Prefab;
    /// <summary> 
    /// 池子的最大数量 
    /// </summary>
    [SerializeField]
    private int MaxCount;

    /// <summary> 
    /// 池子内所有对象的集合 
    /// </summary>
    [System.NonSerialized]
    private List<GameObject> m_PrefabList = new List<GameObject>();

    public bool Contains(GameObject go)
    {
        return m_PrefabList.Contains(go);
    }

    /// <summary> 
    /// 从池子内获取游戏对象 
    /// </summary>
    public GameObject GetObject()
    {
        GameObject go = null;
        for (int i = 0; i < m_PrefabList.Count; i++)
        {
            if (!m_PrefabList[i].activeSelf)
            {
                go = m_PrefabList[i];
                go.SetActive(true);
                break;
            }
        }

        if (go == null)
        {
            if (m_PrefabList.Count >= MaxCount)
            {
                GameObject.Destroy(m_PrefabList[0]);
                m_PrefabList.RemoveAt(0);
            }
            go = GameObject.Instantiate<GameObject>(Prefab);
            m_PrefabList.Add(go);
        }
        go.SendMessage("BeforeGetObject", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    /// <summary> 
    /// 隐藏指定的游戏对象 
    /// </summary>
    public void HideObject(GameObject go)
    {
        if (m_PrefabList.Contains(go))
        {
            go.SendMessage("BeforeHideObject", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    /// <summary> 
    /// 隐藏该池子的全部游戏对象 
    /// </summary>
    public void HideAllObject()
    {
        for (int i = 0; i < m_PrefabList.Count; i++)
        {
            if (m_PrefabList[i].activeSelf)
                HideObject(m_PrefabList[i]);
        }
    }

    public void InitPool()
    {
        m_PrefabList = new List<GameObject>();
    }
}

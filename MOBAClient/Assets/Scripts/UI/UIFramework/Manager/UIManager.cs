using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

/// <summary>
/// 管理UI面板
/// 1.主要的面板放在栈内，使用PushPanel和PopPanel管理
/// 2.并行的子面板，不适合放在栈内的，
/// 使用LoadPanel加载后，通过UIBasePanel的ShowPanel和HidePanel自行管理
/// </summary>
public class UIManager
{
    private static UIManager m_Instance = null;

    public static UIManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new UIManager();
            }
            return m_Instance;
        }
       
    }

    private Transform m_CanvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            m_CanvasTransform = m_CanvasTransform ?? GameObject.Find("Canvas").transform;
            return m_CanvasTransform;
        }
    }

    // 储存所有面板Prefab的路径
    private Dictionary<UIPanelType, string> m_PanelPathDict;
    // 储存所有实例化的面板基类
    private Dictionary<UIPanelType, UIBasePanel> m_PanelDict;
    // 储存栈空间显示的面板
    private Stack<UIBasePanel> m_PanelStack;

    public UIManager()
    {
        m_PanelStack = new Stack<UIBasePanel>();
        m_PanelDict = new Dictionary<UIPanelType, UIBasePanel>();
        ParseUIPanelTypeJson();
    }

    /// <summary>
    /// 入栈并显示面板
    /// </summary>
    public void PushPanel(UIPanelType panelType)
    {
        UIBasePanel panel = GetPanel(panelType);

        if (m_PanelStack.Count > 0)
        {
            UIBasePanel topPanel = m_PanelStack.Peek();
            if (topPanel.name == panel.name)
                return;

            topPanel.OnPause();
        }
        panel.transform.SetAsLastSibling();
        m_PanelStack.Push(panel);
        panel.OnEnter();
    }

    /// <summary>
    /// 出栈并移除显示
    /// </summary>
    public void PopPanel()
    {
        if (m_PanelStack.Count <= 0) return;
        UIBasePanel topPanel = m_PanelStack.Pop();
        topPanel.OnExit();

        if (m_PanelStack.Count <= 0) return;
        topPanel = m_PanelStack.Peek();
        topPanel.OnResume();
    }

    /// <summary>
    /// 清除所有栈中的界面
    /// </summary>
    public void ClearStack()
    {
        while (m_PanelStack.Count > 0)
        {
            m_PanelStack.Pop().OnExit();
        }
    }

    /// <summary>
    /// 加载面板 用于不适合放在栈内的面板
    /// 和ShowPanel，HidePanel一起使用
    /// </summary>
    public UIBasePanel LoadPanel(UIPanelType panelType)
    {
        UIBasePanel panel = GetPanel(panelType);
        panel.HidePanel();

        return panel;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="panelType"></param>
    public void ShopPanel(UIPanelType panelType)
    {
        GetPanel(panelType).ShowPanel();
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelType"></param>
    public void HidePanel(UIPanelType panelType)
    {
        GetPanel(panelType).HidePanel();
    }

    /// <summary>
    /// 解析面板配置文件
    /// </summary>
    private void ParseUIPanelTypeJson()
    {
        m_PanelPathDict = new Dictionary<UIPanelType, string>();
        TextAsset asset = Resources.Load<TextAsset>("UIPanelType");

        List<UIPanelInfo> infoList = JsonMapper.ToObject<List<UIPanelInfo>>(asset.text);
        for (int i = 0; i < infoList.Count; i++)
        {
            UIPanelInfo info = infoList[i];
            m_PanelPathDict.Add(info.PanelType, info.Path);
        }
    }

    /// <summary>
    /// 获得面板 没有则加载
    /// </summary>
    public UIBasePanel GetPanel(UIPanelType panelType)
    {
        UIBasePanel panel = m_PanelDict.ExTryGet(panelType);
        if (panel == null)
        {
            // 如果当前没有保存这个面板，则通过prefab实例化
            string path = m_PanelPathDict.ExTryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransform, false);
            m_PanelDict.Add(panelType, instPanel.GetComponent<UIBasePanel>());
            return instPanel.GetComponent<UIBasePanel>();
        }
        else
        {
            return panel;
        }
    }

    /// <summary>
    /// 清除所有面板
    /// </summary>
    public void ClearAll()
    {
        m_CanvasTransform = null;
        m_PanelStack.Clear();
        m_PanelDict.Clear();
    }
}

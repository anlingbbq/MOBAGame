using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

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
    // 储存显示的面板
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
    public UIBasePanel PushPanel(UIPanelType panelType)
    {
        // 暂停上一级的页面
        if (m_PanelStack.Count > 0)
        {
            UIBasePanel topPanel = m_PanelStack.Peek();
            topPanel.OnPause();
        }
        UIBasePanel panel = GetPanel(panelType);
        panel.transform.SetAsLastSibling();
        panel.OnEnter();
        m_PanelStack.Push(panel);

        return panel;
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
}

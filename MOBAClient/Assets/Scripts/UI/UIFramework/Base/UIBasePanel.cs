using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBasePanel : MonoBehaviour
{
    protected CanvasGroup m_CanvasGroup;

    public virtual void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        if (m_CanvasGroup == null)
        {
            Log.Error("UIPanel 没有挂载CanvasGroup组件");
            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    #region 界面生命周期

    public virtual void OnEnter()
    {
        m_CanvasGroup.alpha = 1;
        m_CanvasGroup.blocksRaycasts = true;
        m_CanvasGroup.interactable = true;
    }

    public virtual void OnPause()
    {
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = false;
    }

    public virtual void OnResume()
    {
        m_CanvasGroup.blocksRaycasts = true;
        m_CanvasGroup.interactable = true;
    }

    public virtual void OnExit()
    {
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = false;
    }

    #endregion


    public virtual void OnClosePanel()
    {
        UIManager.Instance.PopPanel();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 匹配界面
/// </summary>
public class MatchPanel : UIBasePanel
{
    [SerializeField]
    private Text TextTime;

    private bool m_Start = false;
    private float m_Minute = 0;
    private float m_Second = 0;

    public void StartMatch()
    {
        TextTime.text = "00:01";
        m_Start = true;
    }

    public void StopMatch()
    {
        m_Minute = 0;
        m_Second = 0;
        m_Start = false;
    }

    void Update()
    {
        if (m_Start)
        {
            m_Second += Time.deltaTime;
            if (m_Second >= 60)
            {
                m_Minute += 1;
            }
            TextTime.text = m_Minute.ToString("00") + " : " + m_Second.ToString("00");
        }
    }

    public void OnBtnCloseClick()
    {
        this.HidePanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        StartMatch();
    }

    public override void OnExit()
    {
        base.OnExit();
        StopMatch();
    }
}

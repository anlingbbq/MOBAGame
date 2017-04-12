using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BattlePanel : UIBasePanel
{
    /// <summary>
    /// 进入游戏的请求
    /// </summary>
    private EnterBattleRequest m_EnterRequest;

    void Start()
    {
        m_EnterRequest = GetComponent<EnterBattleRequest>();
        OnEnter();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_EnterRequest.SendRequest();
    }
}

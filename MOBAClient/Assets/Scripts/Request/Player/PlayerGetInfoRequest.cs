using System;
using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;

/// <summary>
/// 获取是否有玩家数据的请求
/// </summary>
public class PlayerGetInfoRequest : BaseRequest
{
    private MainMenuPanel m_MainPanel;

    protected override void Start()
    {
        base.Start();
        m_MainPanel = GetComponent<MainMenuPanel>();
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_MainPanel.OnGetInfoRequest(response);
    }
}

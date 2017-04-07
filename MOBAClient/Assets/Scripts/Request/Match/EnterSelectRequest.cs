using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class EnterSelectRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    public override void Start()
    {
        base.Start();
        m_SelectPanel = GetComponent<SelectPanel>();
    }

    /// <summary>
    /// 当有其他客户端进入时
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        int playerId = (int)response.Parameters[(byte)ParameterCode.PlayerId];
        m_SelectPanel.OnEnterSelect(playerId);
    }
}

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

    private SelectModel[] team1;
    private SelectModel[] team2;

    public override void Start()
    {
        this.OpCode = OperationCode.EnterSelect;
        base.Start();
        m_SelectPanel = GetComponent<SelectPanel>();
    }

    public override void DefalutRequest()
    {
        PhotonEngine.Peer.OpCustom((byte) OperationCode.EnterSelect, null, true);
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

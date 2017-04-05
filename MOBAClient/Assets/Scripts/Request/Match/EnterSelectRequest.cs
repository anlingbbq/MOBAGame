using System;
using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class EnterSelectRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    void Start()
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
        m_SelectPanel.OnEnterSelect(response);
    }
}

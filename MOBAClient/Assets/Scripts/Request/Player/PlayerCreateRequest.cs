using System;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerCreateRequest : BaseRequest
{
    private CreatePlayerPanel m_CreatePlayerPanel;

    protected override void Start()
    {
        base.Start();
        m_CreatePlayerPanel = GetComponent<CreatePlayerPanel>();
    }

    /// <summary>
    /// 发送创建玩家的请求
    /// </summary>
    /// <param name="name"></param>
    public void SendCreateRequest(string name)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)(ParameterCode.PlayerName), name);
        SendRequest(data);
    }
    
    public override void OnOperationResponse(OperationResponse response)
    {
        m_CreatePlayerPanel.OnCreateResponse(response);
    }
}

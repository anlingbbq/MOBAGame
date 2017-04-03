using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// 请求添加好友
/// </summary>
public class PlayerAddFriendRequest : BaseRequest
{
    private AddRequestPanel m_AddFriendPanel;

    [HideInInspector]
    public string Username;

    public override void Start()
    {
        this.OpCode = OperationCode.PlayerAddRequest;
        base.Start();

        m_AddFriendPanel = GetComponent<AddRequestPanel>();
    }

    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.PlayerName, Username);
        PhotonEngine.Peer.OpCustom((byte) this.OpCode, data, true);
    }

    /// <summary>
    /// 这里处理的是能不能添加好友 而不是是否同意
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        m_AddFriendPanel.OnAddRequest(response);
    }
}

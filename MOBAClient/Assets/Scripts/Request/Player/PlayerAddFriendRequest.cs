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
    
    public override void Start()
    {
        base.Start();
        m_AddFriendPanel = GetComponent<AddRequestPanel>();
    }

    /// <summary>
    /// 发送添加好友的请求
    /// </summary>
    /// <param name="name"></param>
    public void SendAddFriendRequest(string name)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.PlayerName, name);
        PhotonEngine.Peer.OpCustom((byte)this.OpCode, data, true);
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

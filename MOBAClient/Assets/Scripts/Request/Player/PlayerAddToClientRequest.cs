using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// 是否同意添加好友的反馈
/// </summary>
public class PlayerAddToClientRequest : BaseRequest
{
    [HideInInspector]
    public bool isAccept;

    // 请求的玩家名称
    [HideInInspector]
    public string FromName;

    // 请求的玩家id
    [HideInInspector]
    public int FromId;

    private AddToClientPanel m_AddPenel;

    public override void Start()
    {
        this.OpCode = OperationCode.PlayerAddToClient;
        base.Start();

        m_AddPenel = GetComponent<AddToClientPanel>();
    }

    /// <summary>
    /// 发送是否添加好友的数据
    /// </summary>
    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.AcceptAddFriend, isAccept);
        data.Add((byte)ParameterCode.PlayerName, FromName);
        data.Add((byte)ParameterCode.PlayerId, FromId);
        PhotonEngine.Peer.OpCustom((byte) this.OpCode, data, true);
    }

    /// <summary>
    /// 这里处理的是 服务器对PlayerAddRequest的响应
    /// 将请求添加好友的玩家数据发送过来
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        m_AddPenel.OnOperationResponse(response);
    }
}

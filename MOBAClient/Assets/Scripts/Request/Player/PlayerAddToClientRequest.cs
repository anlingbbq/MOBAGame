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
    public string PlayerName;
    [HideInInspector]
    public bool isAccept;

    private ResponseAddPanel m_AddPenel;

    public override void Start()
    {
        this.OpCode = OperationCode.PlayerAddToClient;
        base.Start();

        m_AddPenel = GetComponent<ResponseAddPanel>();
    }

    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.AcceptAddFriend, isAccept);
        data.Add((byte)ParameterCode.PlayerName, PlayerName);
        PhotonEngine.Peer.OpCustom((byte) this.OpCode, data, true);
    }

    /// <summary>
    /// 这里处理的是 服务器对PlayerAddRequest的响应
    /// 将请求添加好友的一方发送过来
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        m_AddPenel.OnOperationResponse(response);
    }
}

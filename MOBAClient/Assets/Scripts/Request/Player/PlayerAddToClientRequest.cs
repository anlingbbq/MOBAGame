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
    private AddToClientPanel m_AddPenel;

    protected override void Start()
    {
        base.Start();
        m_AddPenel = GetComponent<AddToClientPanel>();
    }

    /// <summary>
    /// 发送是否添加好友的数据
    /// </summary>
    public void SendAddToClientRequest(bool isAccept, string name, int id)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.AcceptAddFriend, isAccept);
        data.Add((byte)ParameterCode.PlayerName, name);
        data.Add((byte)ParameterCode.PlayerId, id);
        SendRequest(data);
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

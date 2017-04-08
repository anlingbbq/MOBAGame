using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// 发起请求和接收服务器响应的基类
/// 使用前需要给OpCode赋值
/// </summary>
public abstract class BaseRequest : MonoBehaviour
{
    public OperationCode OpCode;

    /// <summary>
    /// 主动发起的请求
    /// </summary>
    public virtual void SendRequest(Dictionary<byte, object> data = null)
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }
    /// <summary>
    /// 收到的服务器响应
    /// </summary>
    /// <param name="response"></param>
    public abstract void OnOperationResponse(OperationResponse response);

    protected virtual void Start ()
    {
		PhotonEngine.Instance.AddRequest(this);
	}

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }
}

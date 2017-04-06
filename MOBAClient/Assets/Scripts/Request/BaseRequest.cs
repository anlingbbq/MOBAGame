using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseRequest : MonoBehaviour
{
    [HideInInspector]
    public OperationCode OpCode;

    /// <summary>
    /// 主动发起的请求
    /// </summary>
    public abstract void DefalutRequest();
    /// <summary>
    /// 收到的服务器响应
    /// </summary>
    /// <param name="response"></param>
    public abstract void OnOperationResponse(OperationResponse response);

    // 子类需要在Start方法或之前, 先给OpCode赋值
    public virtual void Start () {
		PhotonEngine.Instance.AddRequest(this);
	}

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }
}

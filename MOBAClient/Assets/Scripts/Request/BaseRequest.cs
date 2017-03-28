using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseRequest : MonoBehaviour
{
    [HideInInspector]
    public OperationCode OpCode;
    public abstract void DefalutRequest();
    public abstract void OnOperationResponse(OperationResponse response);

    // 子类需要重写Start方法, 先给OpCode赋值
    public virtual void Start () {
		PhotonEngine.Instance.AddRequest(this);
	}

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }
}

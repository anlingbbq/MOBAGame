using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class DestroySelectRequest : BaseRequest
{
    void Start()
    {
        this.OpCode = OperationCode.DestroySelect;
        base.Start();
    }

    public override void DefalutRequest()
    {
    }

    /// <summary>
    /// 当选人的房间被摧毁时调用
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
    }
}

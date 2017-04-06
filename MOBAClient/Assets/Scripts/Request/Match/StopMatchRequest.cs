using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class StopMatchRequest : BaseRequest
{
    public override void Start()
    {
        this.OpCode = OperationCode.StopMatch;
        base.Start();
    }

    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.PlayerId, GameData.player.Id);
        PhotonEngine.Peer.OpCustom((byte) this.OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        // 什么也不需要做
    }
}

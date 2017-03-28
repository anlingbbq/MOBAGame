using System;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerCreateRequest : BaseRequest
{
    private CreatePlayerPanel m_CreatePlayerPanel;

    [HideInInspector]
    public string PlayerName;

    public override void Start()
    {
        this.OpCode = OperationCode.PlayerCreate;
        base.Start();

        m_CreatePlayerPanel = GetComponent<CreatePlayerPanel>();
    }

    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)(ParameterCode.PlayerName), PlayerName);
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {

    }
}

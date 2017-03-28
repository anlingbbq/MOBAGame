using System;
using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;

public class PlayerGetInfoRequest : BaseRequest
{
    private LoginPanel m_LoginPanel;

    public override void Start()
    {
        this.OpCode = OperationCode.PlayerGetInfo;
        base.Start();

        m_LoginPanel = GetComponent<LoginPanel>();
    }

    public override void DefalutRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_LoginPanel.OnPlayerInfoResponse(response);
    }
}

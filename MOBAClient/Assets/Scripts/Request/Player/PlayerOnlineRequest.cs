using System;
using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerOnlineRequest : BaseRequest
{
    private MainMenuPanel m_MainPanel;
    
    public override void Start()
    {
        OpCode = OperationCode.PlayerOnline;
        base.Start();

        m_MainPanel = GetComponent<MainMenuPanel>();
    }

    public override void DefalutRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_MainPanel.OnOnline(response);
    }
}

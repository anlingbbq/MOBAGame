using System;
using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;

public class PlayerGetInfoRequest : BaseRequest
{
    private MainMenuPanel m_MainPanel;

    void Awake()
    {
        this.OpCode = OperationCode.PlayerGetInfo;
    }

    public override void Start()
    {
        base.Start();

        m_MainPanel = GetComponent<MainMenuPanel>();
    }

    public override void DefalutRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_MainPanel.OnGetInfoRequest(response);
    }
}

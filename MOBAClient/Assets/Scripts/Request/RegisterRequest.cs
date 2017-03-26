using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    [HideInInspector]
    public string Username;
    [HideInInspector]
    public string Password;

    private RegisterPanel m_RegisterPanel;

    public override void Start()
    {
        this.OpCode = OperationCode.Register;
        base.Start();

        m_RegisterPanel = GetComponent<RegisterPanel>();
    }

    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Username, Username);
        data.Add((byte)ParameterCode.Password, Password);
        PhotonEngine.Peer.OpCustom((byte) OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_RegisterPanel.OnRegisterResponse((ReturnCode)response.ReturnCode);
    }
}

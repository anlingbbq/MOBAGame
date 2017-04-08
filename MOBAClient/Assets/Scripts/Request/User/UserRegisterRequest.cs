using System.Collections;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public class UserRegisterRequest : BaseRequest
{
    private RegisterPanel m_RegisterPanel;

    protected override void Start()
    {
        base.Start();
        m_RegisterPanel = GetComponent<RegisterPanel>();
    }

    public void SendRegisterRequest(string username, string password)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Username, username);
        data.Add((byte)ParameterCode.Password, password);
        SendRequest(data);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_RegisterPanel.OnRegisterResponse(response);
    }
}

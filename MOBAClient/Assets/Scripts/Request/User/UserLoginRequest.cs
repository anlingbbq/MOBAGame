using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class UserLoginRequest : BaseRequest
{
    private LoginPanel m_LoginPanel;

    protected override void Start()
    {
        base.Start();
        m_LoginPanel = GetComponent<LoginPanel>();
    }

    /// <summary>
    /// 发送登录请求
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public void SendLoginRequest(string username, string password)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Username, username);
        data.Add((byte)ParameterCode.Password, password);
        SendRequest(data);
    }
    
    public override void OnOperationResponse(OperationResponse response)
    {
        m_LoginPanel.OnLoginResponse(response);
    }
}

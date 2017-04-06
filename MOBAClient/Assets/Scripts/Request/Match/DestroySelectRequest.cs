using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class DestroySelectRequest : BaseRequest
{
    private MainMenuPanel m_MainMenuPanel;

    public override void Start()
    {
        this.OpCode = OperationCode.DestroySelect;
        base.Start();
        m_MainMenuPanel = GetComponent<MainMenuPanel>();
    }

    /// <summary>
    /// 不需要
    /// </summary>
    public override void DefalutRequest()
    {
    }

    /// <summary>
    /// 当选人的房间被摧毁时调用
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        m_MainMenuPanel.OnDestroySelectRoom();
    }
}

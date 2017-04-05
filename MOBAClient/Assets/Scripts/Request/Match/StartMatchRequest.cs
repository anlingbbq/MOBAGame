using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class StartMatchRequest : BaseRequest
{
    private MainMenuPanel m_MainMenuPanel;

    public override void Start()
    {
        this.OpCode = OperationCode.StartMatch;
        base.Start();

        m_MainMenuPanel = GetComponent<MainMenuPanel>();
    }


    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.PlayerId, GameData.player.Id);
        PhotonEngine.Peer.OpCustom((byte) OpCode, data, true);
    }

    /// <summary>
    /// 这里并没有实现多人匹配 
    /// </summary>
    /// <param name="ids"></param>
    public void MultiRequest(int[] ids)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.PlayerIds, ids);
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    /// <summary>
    /// 匹配完成 进入选择界面
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        m_MainMenuPanel.OnMatchComplete(response);
    }
}

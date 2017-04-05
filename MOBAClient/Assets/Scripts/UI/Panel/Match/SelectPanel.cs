using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选人界面
/// </summary>
public class SelectPanel : UIBasePanel
{
    [SerializeField]
    private Text TextContent;

    private EnterSelectRequest m_SelectRequest;

	public override void Awake()
    {
        base.Awake();
        m_SelectRequest = GetComponent<EnterSelectRequest>();
	}

    /// <summary>
    /// 当有玩家进入选择界面时处理
    /// </summary>
    public void OnEnterSelect(OperationResponse response)
    {
        int playerId = (int) response.Parameters.ExTryGet((byte)ParameterCode.PlayerId);
        string playerName = response.Parameters.ExTryGet((byte)ParameterCode.PlayerName) as string;
        TextContent.text += "<color=#ffff00>" + playerName + "进入房间</color>\n";
    }

    /// <summary>
    /// 获取当前房间内的信息
    /// </summary>
    /// <param name="response"></param>
    public void OnGetSelectInfo(OperationResponse response)
    {
        //SelectModel[] teamOneModels = response.Parameters.ExTryGet((byte)ParameterCode.TeamOneData);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 发送进入选人房间的消息
        m_SelectRequest.DefalutRequest();
    }
}

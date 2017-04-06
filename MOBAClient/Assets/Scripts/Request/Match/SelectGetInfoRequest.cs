using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using Common.OpCode;
using ExitGames.Client.Photon;
using LitJson;

public class SelectGetInfoRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    /// <summary>
    /// 保存队伍1的数据
    /// </summary>
    private SelectModel[] m_Team1;
    /// <summary>
    /// 保存队伍2的数据
    /// </summary>
    private SelectModel[] m_Team2;
    /// <summary>
    /// 玩家的队伍id
    /// </summary>
    private int m_TeamId;

    public override void Start()
    {
        this.OpCode = OperationCode.SelectGetInfo;
        base.Start();

        m_SelectPanel = GetComponent<SelectPanel>();
    }

    /// <summary>
    /// 不需要
    /// </summary>
    public override void DefalutRequest()
    {
    }

    /// <summary>
    /// 接收选人房间内当前的数据
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取队伍数据
        m_Team1 = JsonMapper.ToObject<SelectModel[]>(
          response.Parameters[(byte)ParameterCode.TeamOneData] as string);
        m_Team2 = JsonMapper.ToObject<SelectModel[]>(
            response.Parameters[(byte)ParameterCode.TeamTwoData] as string);
        
        // 判断玩家在哪个队伍
        GetTeamId(m_Team1, m_Team2);

        // 更新界面
        m_SelectPanel.UpdateView(m_TeamId, m_Team1, m_Team2);
    }

    /// <summary>
    /// 获取玩家的队伍id
    /// </summary>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    /// <returns></returns>
    public void GetTeamId(SelectModel[] team1, SelectModel[] team2)
    {
        int playerId = GameData.player.Id;
        for (int i = 0; i < team1.Length; i++)
        {
            if (playerId == team1[i].PlayerId)
            {
                m_TeamId = 1;
                return;
            }
        }
        for (int i = 0; i < team2.Length; i++)
        {
            if (playerId == team2[i].PlayerId)
            {
                m_TeamId = 2;
                return;
            }
        }
    }

    /// <summary>
    /// 新的玩家进入房间时 更新数据
    /// </summary>
    /// <param name="playerId"></param>
    public void OnEnterSelect(int playerId)
    {
        foreach (SelectModel model in m_Team1)
        {
            if (model.PlayerId == playerId)
            {
                model.IsEnter = true;
                if (m_TeamId == 1)
                    m_SelectPanel.EnterTextPrompt(model.PlayerName);
                break;
            }
        }
        foreach (SelectModel model in m_Team2)
        {
            if (model.PlayerId == playerId)
            {
                model.IsEnter = true;
                if (m_TeamId == 2)
                    m_SelectPanel.EnterTextPrompt(model.PlayerName);
                break;
            }
        }

        // 更新界面
        m_SelectPanel.UpdateView(m_TeamId, m_Team1, m_Team2);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选人界面
/// </summary>
public class SelectPanel : UIBasePanel, IResourceListener
{
    [SerializeField]
    private SelectPlayerItem[] OurTeam;

    [SerializeField]
    private SelectPlayerItem[] EnemyTeam;

    /// <summary>
    /// 聊天内容
    /// </summary>
    [SerializeField]
    private Text TextContent;

    /// <summary>
    /// 接收新进入房间的玩家信息
    /// </summary>
    private EnterSelectRequest m_EnterRequest;

    /// <summary>
    /// 房间的数据处理主要在这里
    /// </summary>
    private SelectGetInfoRequest m_GetInfoRequest;

	public override void Awake()
    {
        base.Awake();
        m_EnterRequest = GetComponent<EnterSelectRequest>();
        m_GetInfoRequest = GetComponent<SelectGetInfoRequest>();

        // 加载头像
        LoadHead();
	}

    /// <summary>
    /// 加载头像图片
    /// </summary>
    public void LoadHead()
    {
        ResourcesManager.Instance.Load(Paths.HEAD_NO_CONNECT, typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.HEAD_ASHE, typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.HEAD_GAREN, typeof(Sprite), this);
    }

    /// <summary>
    /// 当有其他玩家进入选择界面时处理
    /// </summary>
    public void OnEnterSelect(int playerId)
    {
        m_GetInfoRequest.OnEnterSelect(playerId);
    }

    /// <summary>
    /// 新玩家进入时的文字提示
    /// </summary>
    /// <param name="playerName"></param>
    public void EnterTextPrompt(string playerName)
    {
        TextContent.text += "<color=#ffff00>" + playerName + "进入房间</color>\n";
    }

    /// <summary>
    /// 更新视图
    /// </summary>
    /// <param name="TeamId">自身的队伍</param>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    public void UpdateView(int TeamId, SelectModel[] team1, SelectModel[] team2)
    {
        if (TeamId == 1)
        {
            for (int i = 0; i < team1.Length; i++)
            {
                OurTeam[i].UpdateView(team1[i]);
            }
            for (int i = 0; i < team2.Length; i++)
            {
                EnemyTeam[i].UpdateView(team2[i]);
            }
        }
        else if (TeamId == 2)
        {
            for (int i = 0; i < team1.Length; i++)
            {
                EnemyTeam[i].UpdateView(team1[i]);
            }
            for (int i = 0; i < team2.Length; i++)
            {
                OurTeam[i].UpdateView(team2[i]);
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 发送进入选人房间的消息
        m_EnterRequest.DefalutRequest();
    }

    public void OnLoaded(string assetName, object asset, AssetType assetType)
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Common.Code;
using Common.Config;
using Common.Dto;
using Common.OpCode;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选人界面
/// </summary>
public class SelectPanel : UIBasePanel
{
    [Header("队伍")]
    [SerializeField]
    private ItemSelectPlayer[] OurTeam;

    [SerializeField]
    private ItemSelectPlayer[] EnemyTeam;

    /// <summary>
    /// 聊天内容
    /// </summary>
    [SerializeField]
    private Text TextContent;

    /// <summary>
    /// 进入房间的请求
    /// </summary>
    private EnterSelectRequest m_EnterRequest;

    /// <summary>
    /// 选择角色的请求
    /// </summary>
    private SelectedRequest m_SelectedRequest;

    /// <summary>
    /// 保存和管理选人的数据
    /// </summary>
    public SelectData SelectData;

	public override void Awake()
    {
        base.Awake();
        m_EnterRequest = GetComponent<EnterSelectRequest>();
        m_SelectedRequest = GetComponent<SelectedRequest>();

        SelectData = new SelectData();
	}

    [Header("英雄")]
    [SerializeField]
    private GameObject ItemHero;
    [SerializeField]
    private Transform GridHero;

    /// <summary>
    /// 保存已加载的英雄项 避免重复创建
    /// </summary>
    private Dictionary<int, ItemHero> ItemHeroDict = new Dictionary<int, ItemHero>();

    /// <summary>
    /// 初始化选择英雄的层
    /// </summary>
    public void InitSelectHeroLayer(List<int> heroIds)
    {
        GameObject go;
        foreach (int id in heroIds)
        {
            if (ItemHeroDict.ContainsKey(id))
                continue;

            go = Instantiate(ItemHero);
            ItemHero hero = go.GetComponent<ItemHero>();
            hero.InitView(HeroData.GetHeroData(id));
            go.transform.SetParent(GridHero);
            go.transform.localScale = Vector3.one;

            ItemHeroDict.Add(id, hero);
        }
    }

    /// <summary>
    /// 当有其他玩家进入选择界面时处理
    /// </summary>
    public void OnEnterSelect(int playerId)
    {
        string name = SelectData.OnEnterSelect(playerId);
        if (name != null) EnterTextPrompt(name);
        UpdateView();
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
    public void UpdateView()
    {
        // 已经选择的英雄
        List<int> selectedHero = new List<int>();

        if (SelectData.TeamId == 1)
        {
            for (int i = 0; i < SelectData.Team1.Length; i++)
            {
                OurTeam[i].UpdateView(SelectData.Team1[i]);
            }
            for (int i = 0; i < SelectData.Team2.Length; i++)
            {
                EnemyTeam[i].UpdateView(SelectData.Team2[i]);
            }
            // 添加到已选择的链表中
            foreach (SelectModel model in SelectData.Team1)
            {
                if (model.HeroId != -1)
                    selectedHero.Add(model.HeroId);
            }
        }
        else if (SelectData.TeamId == 2)
        {
            for (int i = 0; i < SelectData.Team1.Length; i++)
            {
                EnemyTeam[i].UpdateView(SelectData.Team1[i]);
            }
            for (int i = 0; i < SelectData.Team2.Length; i++)
            {
                OurTeam[i].UpdateView(SelectData.Team2[i]);
            }
            // 添加到已选择的链表中
            foreach (SelectModel model in SelectData.Team2)
            {
                if (model.HeroId != -1)
                    selectedHero.Add(model.HeroId);
            }
        }

        // 禁用英雄
        foreach (ItemHero hero in ItemHeroDict.Values)
        {
            // 如果这个英雄已经被选择了
            if (selectedHero.Contains(hero.HeroId))
                hero.Interactable = false;
            else
                hero.Interactable = true;
        }
    }

    /// <summary>
    /// 发送选人的请求
    /// </summary>
    /// <param name="heroId"></param>
    public void SendSelectedRequest(int heroId)
    {
        m_SelectedRequest.SendSelectedRequest(heroId);
    }

    /// <summary>
    /// 处理选人的响应
    /// </summary>
    public void OnSelected(OperationResponse response)
    {
        if (response.ReturnCode != (short) ReturnCode.Falied)
        {
            int playerId = (int)response.Parameters[(byte)ParameterCode.PlayerId];
            int heroId = (int)response.Parameters[(byte)ParameterCode.HeroId];
            // 刷新队伍数据
            SelectData.OnSelected(playerId, heroId);
            UpdateView();
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 发送进入选人房间的消息
        m_EnterRequest.SendRequest();
    }
}

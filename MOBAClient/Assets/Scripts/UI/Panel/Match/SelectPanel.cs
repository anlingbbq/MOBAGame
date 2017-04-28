using System;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选人界面
/// </summary>
public class SelectPanel : UIBasePanel
{
    #region UI

    /// <summary>
    /// 我方队伍ui
    /// </summary>
    [Header("队伍")]
    [SerializeField]
    private ItemSelectPlayer[] OurTeam;

    /// <summary>
    /// 地方队伍ui
    /// </summary>
    [SerializeField]
    private ItemSelectPlayer[] EnemyTeam;


    /// <summary>
    /// 确认按钮
    /// </summary>
    [Header("其他")]
    [SerializeField]
    private Button BtnReady;
    /// <summary>
    /// 聊天的所有内容
    /// </summary>
    [SerializeField]
    private Text TextContent;
    /// <summary>
    /// 聊天输入框
    /// </summary>
    [SerializeField]
    private InputField InputTalk;
    /// <summary>
    /// 发送聊天按钮
    /// </summary>
    [SerializeField]
    private Button BtnTalk;

    #endregion

    #region 请求

    /// <summary>
    /// 进入房间的请求
    /// </summary>
    private EnterSelectRequest m_EnterRequest;
    /// <summary>
    /// 选择角色的请求
    /// </summary>
    private SelectedRequest m_SelectedRequest;
    /// <summary>
    /// 准备完成的消息
    /// </summary>
    private BeReaySelectRequest m_BeReadyRequest;
    /// <summary>
    /// 聊天消息的请求
    /// </summary>
    private TalkInSelectRequest m_TalkReqeust;

    #endregion

    public override void Awake()
    {
        base.Awake();
        m_EnterRequest = GetComponent<EnterSelectRequest>();
        m_SelectedRequest = GetComponent<SelectedRequest>();
        m_BeReadyRequest = GetComponent<BeReaySelectRequest>();
        m_TalkReqeust = GetComponent<TalkInSelectRequest>();

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
        string name = SelectData.Instance.OnEnterSelect(playerId);
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
    public void UpdateView()
    {
        // 已经选择的英雄
        List<int> selectedHero = new List<int>();

        if (SelectData.Instance.TeamId == 1)
        {
            for (int i = 0; i < SelectData.Instance.Team1.Length; i++)
            {
                OurTeam[i].UpdateView(SelectData.Instance.Team1[i]);
            }
            for (int i = 0; i < SelectData.Instance.Team2.Length; i++)
            {
                EnemyTeam[i].UpdateView(SelectData.Instance.Team2[i]);
            }
            // 添加到已选择的链表中
            foreach (DtoSelect model in SelectData.Instance.Team1)
            {
                if (model.HeroId != -1)
                    selectedHero.Add(model.HeroId);
            }
        }
        else if (SelectData.Instance.TeamId == 2)
        {
            for (int i = 0; i < SelectData.Instance.Team1.Length; i++)
            {
                EnemyTeam[i].UpdateView(SelectData.Instance.Team1[i]);
            }
            for (int i = 0; i < SelectData.Instance.Team2.Length; i++)
            {
                OurTeam[i].UpdateView(SelectData.Instance.Team2[i]);
            }
            // 添加到已选择的链表中
            foreach (DtoSelect model in SelectData.Instance.Team2)
            {
                if (model.HeroId != -1)
                    selectedHero.Add(model.HeroId);
            }
        }

        // 禁用英雄
        foreach (ItemHero hero in ItemHeroDict.Values)
        {
            // 如果玩家已经准备了
            if (BtnReady.interactable == false && hasSelected)
            {
                hero.Interactable = false;
                continue;
            }

            // 如果这个英雄已经被选择了
            if (selectedHero.Contains(hero.HeroId))
                hero.Interactable = false;
            else
                hero.Interactable = true;
        }
    }

    /// <summary>
    /// 是否有选择
    /// </summary>
    private bool hasSelected = false;

    /// <summary>
    /// 点击英雄头像的回调
    /// </summary>
    /// <param name="heroId"></param>
    public void OnSelectHeroClick(int heroId)
    {
        hasSelected = true;
        BtnReady.interactable = true;
        m_SelectedRequest.SendSelectedRequest(heroId);
    }

    /// <summary>
    /// 点击准备按钮的回调
    /// </summary>
    public void OnReadyClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_READY);

        m_BeReadyRequest.SendRequest();
        BtnReady.interactable = false;
    }

    /// <summary>
    /// 点击聊天按钮的回调
    /// </summary>
    public void OnTalkClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        m_TalkReqeust.SendTalkRequesst(SelectData.Instance.TeamId, 
            GameData.Player.Name + ":" + InputTalk.text);

        InputTalk.text = "";
    }

    /// <summary>
    /// 添加聊天内容
    /// </summary>
    public void OnTalk(int teamId, string str)
    {
        if (SelectData.Instance.TeamId == teamId)
            TextContent.text += str + "\n";
        else
            TextContent.text += "<color=#ff0000>" + str + "</color>\n";
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 禁用准备按钮
        BtnReady.interactable = false;
        // 清空聊天框
        TextContent.text = "";
        // 发送进入选人房间的消息
        m_EnterRequest.SendRequest();
    }
}

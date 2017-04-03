using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.DTO;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登陆后的主界面
/// </summary>
public class MainMenuPanel : UIBasePanel
{
    [Header("玩家属性")]
    [SerializeField]
    private Text TextName;
    [SerializeField]
    private Text TextLv;
    [SerializeField]
    private Text TextPower;
    [SerializeField]
    private Slider SliderExp;

    // 获取玩家信息的消息处理
    private PlayerGetInfoRequest m_InfoRequest;
    // 玩家上线的消息处理
    private PlayerOnlineRequest m_OnlineRequest;

    public override void Awake()
    {
        base.Awake();
        m_InfoRequest = GetComponent<PlayerGetInfoRequest>();
        m_OnlineRequest = GetComponent<PlayerOnlineRequest>();
        m_StartMatchRequest = GetComponent<PlayerStartMatchRequest>();

        SoundManager.Instance.StopBgMusic();
    }

    /// <summary>
    /// 获取是否存在玩家的信息
    /// </summary>
    /// <param name="response"></param>
    public void OnGetInfoRequest(OperationResponse response)
    {
        if ((ReturnCode)response.ReturnCode == ReturnCode.Suceess)
        {
            // 存在角色 发送在线请求
            m_OnlineRequest.DefalutRequest();
        }
        else if ((ReturnCode)response.ReturnCode == ReturnCode.Empty)
        {
            // 打开创建角色的面板
            UIManager.Instance.PushPanel(UIPanelType.CreatePlayer);
        }
        else if ((ReturnCode)response.ReturnCode == ReturnCode.Falied)
        {
            UIManager.Instance.PopPanel();

            TipPanel.SetContent(response.DebugMessage);
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
    }

    /// <summary>
    /// 获取玩家数据 处理玩家上线 实际上是主界面的初始化工作
    /// </summary>
    /// <param name="response"></param>
    public void OnOnline(OperationResponse response)
    {
        // 获取角色数据
        DtoPlayer dtoPlayer = JsonMapper.ToObject<DtoPlayer>
            (response.Parameters.ExTryGet((byte)ParameterCode.DtoPlayer) as string);

        GameData.player = dtoPlayer;

        TextName.text = dtoPlayer.Name;
        TextLv.text = dtoPlayer.Lv.ToString();
        TextPower.text = dtoPlayer.Power.ToString();

        // 加载子界面
        m_FriendListPanel = UIManager.Instance.LoadPanel(UIPanelType.FriendList) as FriendListPanel;
        m_AddRequestPanel = UIManager.Instance.LoadPanel(UIPanelType.AddRequest) as AddRequestPanel;
        m_AddToClientPanel = UIManager.Instance.LoadPanel(UIPanelType.AddToClient) as AddToClientPanel;
        m_MatchPanel = UIManager.Instance.LoadPanel(UIPanelType.Match) as MatchPanel;

        // 关闭遮罩面板
        UIManager.Instance.PopPanel();
    }

    #region 好友模块

    // 请求添加好友的界面
    private AddRequestPanel m_AddRequestPanel;
    // 回应添加好友的界面 
    private AddToClientPanel m_AddToClientPanel;
    // 好友列表的界面
    private FriendListPanel m_FriendListPanel;

    private bool m_IsOpenAddFriend;
    /// <summary>
    /// 控制添加好友面板的显示 
    /// </summary>
    public void OnBtnAddFriendsClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (m_IsOpenAddFriend)
        {
            m_AddRequestPanel.HidePanel();
            m_IsOpenAddFriend = false;
        }
        else
        {
            m_AddToClientPanel.HidePanel();
            if (m_IsOpenFriendList)
            {
                m_FriendListPanel.HidePanel();
                m_IsOpenFriendList = false;
            }
            m_IsOpenAddFriend = true;
            m_AddRequestPanel.ShowPanel();
        }
    }

    private bool m_IsOpenFriendList;
    /// <summary>
    /// 控制好友列表面板的显示 
    /// </summary>
    public void OnBtnFriendListClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (m_IsOpenFriendList)
        {
            m_FriendListPanel.HidePanel();
            m_IsOpenFriendList = false;
        }
        else
        {
            m_AddToClientPanel.HidePanel();
            if (m_IsOpenAddFriend)
            {
                m_AddRequestPanel.HidePanel();
                m_IsOpenAddFriend = false;
            }
            m_IsOpenFriendList = true;
            m_FriendListPanel.ShowPanel();
        }
    }

    /// <summary>
    /// 获取添加好友的结果
    /// </summary>
    /// <param name="response"></param>
    public void OnAddResult(OperationResponse response)
    {
        if (response.ReturnCode == (short)ReturnCode.Falied)
        {
            // 对方拒绝了 但我什么也不敢做
        }
        else if (response.ReturnCode == (short)ReturnCode.Suceess)
        {
            // 添加新的好友
            DtoFriend friend = JsonMapper.ToObject<DtoFriend>(response.Parameters[(byte)ParameterCode.DtoFriend] as string);

            FriendListPanel panel = (FriendListPanel)UIManager.Instance.GetPanel(UIPanelType.FriendList);
            panel.AddFriend(friend);
        }
    }

    /// <summary>
    /// 好友上线或下线的处理
    /// </summary>
    /// <param name="response"></param>
    public void OnFriendStateChange(OperationResponse response)
    {
        DtoFriend friend = JsonMapper.ToObject<DtoFriend>(response.Parameters[(byte)ParameterCode.DtoFriend] as string);
        FriendListPanel panel = (FriendListPanel)UIManager.Instance.GetPanel(UIPanelType.FriendList);
        panel.UpdateFriendItem(friend);
    }

    #endregion

    #region 匹配模块

    // 匹配界面
    private MatchPanel m_MatchPanel;
    [Header("匹配模块")]
    // 单人匹配按钮
    [SerializeField]
    private Button m_BtnSingleMatch;
    // 多人匹配按钮
    [SerializeField]
    private Button m_BtnMultiMatch;

    // 匹配的请求
    private PlayerStartMatchRequest m_StartMatchRequest;

    /// <summary>
    /// 设置匹配按钮的激活状态
    /// </summary>
    /// <param name="isActive"></param>
    public void SetMatchBtnActive(bool isActive)
    {
        m_BtnSingleMatch.interactable = isActive;
        m_BtnMultiMatch.interactable = isActive;
    }

    /// <summary>
    /// 单人匹配游戏 
    /// </summary>
    public void OnBtnSinglePlayClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        m_MatchPanel.ShowPanel();
        m_StartMatchRequest.DefalutRequest();
        SetMatchBtnActive(false);
    }

    /// <summary>
    /// 多人匹配游戏 
    /// </summary>
    public void OnBtnMultiPlayClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

       // TODO 没有多人匹配的实现
    }

    /// <summary>
    /// 匹配完成 进入选择界面
    /// </summary>
    public void OnMatchComplete(OperationResponse response)
    {
        if ((ReturnCode) response.ReturnCode == ReturnCode.Suceess)
        {
            // 停止匹配
            m_MatchPanel.HidePanel();
            SetMatchBtnActive(true);
        }
    }

    #endregion

    public override void OnEnter()
    {
        base.OnEnter();

        UIManager.Instance.PushPanel(UIPanelType.Mask);
        m_InfoRequest.DefalutRequest();
    }

    public override void OnPause()
    {
        // 打开子界面时 不屏蔽主界面
    }

    public override void OnResume()
    {
        // 打开子界面时 不屏蔽主界面
    }
}

using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.DTO;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

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

    private PlayerGetInfoRequest m_InfoRequest;
    private PlayerOnlineRequest m_OnlineRequest;

    public override void Awake()
    {
        base.Awake();
        m_InfoRequest = GetComponent<PlayerGetInfoRequest>();
        m_OnlineRequest = GetComponent<PlayerOnlineRequest>();

        SoundManager.Instance.StopBgMusic();
    }

    #region 点击回掉

    // 匹配单人游戏
    public void OnBtnSinglePlayClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);
    }

    // 匹配多人游戏
    public void OnBtnMultiPlayClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);
    }
    
    private bool m_IsOpenAddFriend;
    // 控制添加好友面板的显示
    public void OnBtnAddFriendsClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (m_IsOpenAddFriend)
        {
            UIManager.Instance.PopPanel();
            m_IsOpenAddFriend = false;
        }
        else
        {
            if (m_IsOpenFriendList)
            {
                UIManager.Instance.PopPanel();
                m_IsOpenFriendList = false;
            }
            m_IsOpenAddFriend = true;
            UIManager.Instance.PushPanel(UIPanelType.AddRequest);
        }
    }

    private bool m_IsOpenFriendList;
    // 控制好友列表面板的显示
    public void OnBtnFriendListClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (m_IsOpenFriendList)
        {
            UIManager.Instance.PopPanel();
            m_IsOpenFriendList = false;
        }
        else
        {
            if (m_IsOpenAddFriend)
            {
                UIManager.Instance.PopPanel();
                m_IsOpenAddFriend = false;
            }
            m_IsOpenFriendList = true;
            UIManager.Instance.PushPanel(UIPanelType.FriendList);
        }
    }

    #endregion

    #region 响应服务器

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

    public void OnOnline(OperationResponse response)
    {
        // 关闭遮罩面板
        UIManager.Instance.PopPanel();

        // 获取角色数据
        DtoPlayer dtoPlayer = JsonMapper.ToObject<DtoPlayer>
            (response.Parameters.ExTryGet((byte)ParameterCode.DtoPlayer) as string);

        GameData.player = dtoPlayer;

        TextName.text = dtoPlayer.Name;
        TextLv.text = dtoPlayer.Lv.ToString();
        TextPower.text = dtoPlayer.Power.ToString();
    }

    public void OnFriendOnlie(OperationResponse response)
    {
        
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

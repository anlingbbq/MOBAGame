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
    [SerializeField]
    private Text TextName;
    [SerializeField]
    private Text TextLv;
    [SerializeField]
    private Text TextPower;
    [SerializeField]
    private Slider SliderExp;
    [SerializeField]
    private Transform LayerFriendList;
    [SerializeField]
    private Transform LayerAddFriend;

    private PlayerGetInfoRequest m_InfoRequest;

    public override void Awake()
    {
        base.Awake();
        m_InfoRequest = GetComponent<PlayerGetInfoRequest>();
    }

    public void OnGetInfoRequest(OperationResponse response)
    {
        if ((ReturnCode)response.ReturnCode == ReturnCode.Suceess)
        {
            // 获取角色数据
            DTOPlayer dtoPlayer = JsonMapper.ToObject<DTOPlayer>(response
                .Parameters
                .ExTryGet((byte)ParameterCode.PlayerDot) as string);

            TextName.text = dtoPlayer.Name;
            TextLv.text = dtoPlayer.Lv.ToString();
            TextPower.text = dtoPlayer.Power.ToString();
        }
        else if ((ReturnCode)response.ReturnCode == ReturnCode.Empty)
        {
            // 打开创建角色的面板
            UIManager.Instance.PushPanel(UIPanelType.CreatePlayer);
        }
        else if ((ReturnCode)response.ReturnCode == ReturnCode.Falied)
        {
            TipPanel.SetContent(response.DebugMessage);
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_InfoRequest.DefalutRequest();
    }
}

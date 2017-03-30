using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayerPanel : UIBasePanel
{
    [SerializeField]
    public InputField InputName;

    private PlayerCreateRequest m_CreateRequest;

    void Start()
    {
        m_CreateRequest = GetComponent<PlayerCreateRequest>();
    }

    #region 点击回掉

    public void OnBtnOkClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (String.IsNullOrEmpty(InputName.text))
            return;

        UIManager.Instance.PushPanel(UIPanelType.Mask);

        // 发送创建玩家的请求
        m_CreateRequest.PlayerName = InputName.text;
        m_CreateRequest.DefalutRequest();
    }

    #endregion

    #region 服务器响应

    public void OnCreateResponse(OperationResponse response)
    {
        // 关闭遮罩面板
        UIManager.Instance.PopPanel();

        // 打开主界面
        UIManager.Instance.PushPanel(UIPanelType.MainMenu);
    }

    #endregion
}

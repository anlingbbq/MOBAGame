using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 创建玩家的界面
/// </summary>
public class CreatePlayerPanel : UIBasePanel
{
    [SerializeField]
    public InputField InputName;

    private PlayerCreateRequest m_CreateRequest;

    void Start()
    {
        m_CreateRequest = GetComponent<PlayerCreateRequest>();
    }

    /// <summary>
    /// 按钮回调
    /// </summary>
    public void OnBtnOkClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (String.IsNullOrEmpty(InputName.text))
            return;

        UIManager.Instance.PushPanel(UIPanelType.Mask);

        // 发送创建玩家的请求
        m_CreateRequest.SendCreateRequest(InputName.text);
    }

    /// <summary>
    /// 处理创建玩家的响应
    /// </summary>
    /// <param name="response"></param>
    public void OnCreateResponse(OperationResponse response)
    {
        // 关闭遮罩面板
        UIManager.Instance.PopPanel();

        // 打开主界面
        UIManager.Instance.PushPanel(UIPanelType.MainMenu);
    }
}

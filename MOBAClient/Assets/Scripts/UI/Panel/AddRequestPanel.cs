using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 请求添加好友的面板
/// </summary>
public class AddRequestPanel : UIBasePanel
{
    [SerializeField]
    private InputField InputName;

    private PlayerAddFriendRequest m_AddRequest;

    void Start()
    {
        m_AddRequest = GetComponent<PlayerAddFriendRequest>();
    }

    #region 点击回掉

    public void OnBtnOk()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (String.IsNullOrEmpty(InputName.text))
            return;

        m_AddRequest.Username = InputName.text;
        m_AddRequest.DefalutRequest();
    }

    #endregion

    #region 服务器响应

    /// <summary>
    /// 获取请求添加好友的响应
    /// </summary>
    /// <param name="response"></param>
    public void OnAddRequest(OperationResponse response)
    {
        if ((ReturnCode) response.ReturnCode == ReturnCode.Falied)
        {
            TipPanel.SetContent(response.DebugMessage);
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
    }

    #endregion

    public override void OnEnter()
    {
        base.OnEnter();

        InputName.ActivateInputField();
    }

    public override void OnExit()
    {
        base.OnExit();

        InputName.text = "";
    }
}

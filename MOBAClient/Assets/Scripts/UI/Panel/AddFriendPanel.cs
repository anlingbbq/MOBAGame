using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class AddFriendPanel : UIBasePanel
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

    public void OnAddFriend(OperationResponse response)
    {
        
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

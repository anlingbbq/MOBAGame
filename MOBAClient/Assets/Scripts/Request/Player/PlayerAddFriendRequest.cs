using System;
using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerAddFriendRequest : BaseRequest
{
    private AddFriendPanel m_AddFriendPanel;

    [HideInInspector]
    public string Username;

    public override void Start()
    {
        this.OpCode = OperationCode.PlayerAdd;
        base.Start();

        m_AddFriendPanel = GetComponent<AddFriendPanel>();
    }

    public override void DefalutRequest()
    {
        // TODO 完善添加好友的功能
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_AddFriendPanel.OnAddFriend(response);
    }
}

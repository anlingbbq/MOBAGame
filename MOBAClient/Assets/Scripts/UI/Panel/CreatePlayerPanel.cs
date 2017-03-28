using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayerPanel : UIBasePanel
{
    public InputField InputName;

    private PlayerCreateRequest m_CreateRequest;

    void Start()
    {
        m_CreateRequest = GetComponent<PlayerCreateRequest>();
    }

    public void OnBtnOkClick()
    {
        if (String.IsNullOrEmpty(InputName.text))
        {
            return;
        }
        m_CreateRequest.PlayerName = InputName.text;
        m_CreateRequest.DefalutRequest();
    }

    public void OnCreateResponse(OperationResponse response)
    {
        Log.Debug("创建成功");
    }
}

using System.Collections;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class ResponseAddPanel : UIBasePanel
{
    [SerializeField]
    private Text TextName;

    [HideInInspector]
    public string PlayerName;

    private PlayerAddToClientRequest m_AddRequest;

    public void OnBtnClick(bool isAccept)
    {
        m_AddRequest.isAccept = isAccept;
        m_AddRequest.PlayerName = PlayerName;
        m_AddRequest.DefalutRequest();
    }

    public void OnOperationResponse(OperationResponse response)
    {
        PlayerName = response.Parameters.ExTryGet((byte)(ParameterCode.PlayerName)) as string;
        TextName.text = PlayerName;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        TextName.text = "";
    }
}

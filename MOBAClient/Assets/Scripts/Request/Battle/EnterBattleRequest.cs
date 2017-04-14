using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;

public class EnterBattleRequest : BaseRequest
{
    private BattlePanel m_BattlePanel;

    protected override void Start()
    {
        base.Start();
        m_BattlePanel = GetComponent<BattlePanel>();
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        // 保存英雄和建筑的数据
        string heroJson = response.Parameters[(byte)ParameterCode.HerosArray] as string;
        string bulidJson = response.Parameters[(byte)ParameterCode.BuildsArray] as string;
        DtoHero[] heros = JsonMapper.ToObject<DtoHero[]>(heroJson);
        DtoBuild[] builds = JsonMapper.ToObject<DtoBuild[]>(bulidJson);

        // 初始化数据
        BattleData.Instance.InitData(heros, builds);

        // 初始化界面
        m_BattlePanel.InitView();
    }
}

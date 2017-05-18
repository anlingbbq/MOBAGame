using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;

public class EnterBattleRequest : BaseRequest
{
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取英雄,建筑,技能的数据
        DtoHero[] heros = JsonMapper.ToObject<DtoHero[]>(response[(byte)ParameterCode.HerosArray] as string);
        DtoBuild[] builds = JsonMapper.ToObject<DtoBuild[]>(response[(byte)ParameterCode.BuildsArray] as string);
        SkillModel[] skills = JsonMapper.ToObject<SkillModel[]>(response[(byte)ParameterCode.SkillArray] as string);
        // 初始化战斗数据
        BattleData.Instance.InitData(heros, builds, skills);

        // 保存物品数据
        string itemJson = response[(byte)ParameterCode.ItemArray] as string;
        GameData.Items = JsonMapper.ToObject<ItemModel[]>(itemJson);

        // 初始化战斗界面
        UIManager.Instance.PushPanel(UIPanelType.Battle);
        // 焦距英雄
        Camera.main.GetComponent<CameraCtrl>().FocusOn(GameData.HeroCtrl.transform);
    }
}

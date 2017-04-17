using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;

public class DamageRequest : BaseRequest
{
    [SerializeField]
    private BattlePanel m_BattlePanel;

    /// <summary>
    ///  处理计算伤害的响应
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        DtoDamage[] damages =JsonMapper.ToObject<DtoDamage[]>(response[(byte)ParameterCode.DtoDamages] as string);
        foreach (DtoDamage item in damages)
        {
            // 获取目标控制器
            int toId = item.ToId;
            BaseCtrl toCtrl = BattleData.Instance.CtrlDict.ExTryGet(toId);
            if (toCtrl == null)
                return;
            toCtrl.Model.CurHp -= item.Damage;
            toCtrl.OnHpChange();
            // 显示伤害数值

            // 如果被攻击的是自己
            if (toId == GameData.Player.Id)
            {
                // 更新ui
                m_BattlePanel.UpdateView((DtoHero)toCtrl.Model);
             
            }

            // 如果死亡了
            if (item.IsDead)
            {
                toCtrl.DeathResponse();
            }
        }
    }
}

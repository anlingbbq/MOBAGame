﻿using System;
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
    /// 发送计算伤害的请求
    /// </summary>
    /// <param name="attackId">攻击者id</param>
    /// <param name="skillId">技能id</param>
    /// <param name="targetArray">目标id数组</param>
    public void SendDamage(int attackId, int skillId, params int[] targetArray)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.AttackId, attackId);
        data.Add((byte)ParameterCode.SkillId, skillId);
        data.Add((byte)ParameterCode.TargetArray, JsonMapper.ToJson(targetArray));

        SendRequest(data);
    }

    /// <summary>
    ///  处理计算伤害的响应
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        DtoDamage[] damages =JsonMapper.ToObject<DtoDamage[]>(response[(byte)ParameterCode.DtoDamages] as string);
        DtoDamage item = null;
        for(int i = 0; i < damages.Length; i++)
        {
            item = damages[i];
            int toId = item.ToId;
            // 获取目标控制器
            BaseCtrl toCtrl = BattleData.Instance.CtrlDict.ExTryGet(toId);
            if (toCtrl == null)
                return;
            toCtrl.Model.CurHp -= item.Damage;
            toCtrl.OnHpChange();

            // 显示伤害数值
            m_BattlePanel.FloatDamage(item.Damage, toCtrl.transform);

            // 如果被攻击的是自己
            if (toId == GameData.Player.Id)
            {
                // 更新ui界面
                m_BattlePanel.UpdateView((DtoHero) toCtrl.Model);
                // TODO 如果死亡了 屏幕灰白
                if (item.IsDead)
                {
                    toCtrl.DeathResponse();
                }
            }
            else
            {
                // 如果目标死亡了
                if (item.IsDead)
                {
                    toCtrl.DeathResponse();

                    // 如果攻击的首要目标死亡
                    if (i == 0)
                    {
                        // 获取攻击者的控制器
                        BaseCtrl ctrl = BattleData.Instance.CtrlDict.ExTryGet(item.FromId);
                        if (ctrl == null)
                            return;
                        // 停止攻击
                        ctrl.StopAttack();
                    }
                }
            }
        }
    }
}
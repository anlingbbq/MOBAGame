using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public class HeroAttackRequest : BaseRequest
{
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获得有英雄攻击的数据
        int fromId = (int)response[(byte)ParameterCode.AttackId];
        int toId = (int)response[(byte)ParameterCode.TargetId];
        int skillId = (int)response[(byte)ParameterCode.SkillId];

        BattleData.Instance.OnAttack(fromId, toId, skillId);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public class AttackRequest : BaseRequest
{
    /// <summary>
    /// 发送攻击的请求
    /// </summary>
    /// <param name="skillId">技能id</param>
    /// <param name="attackId">攻击者的id</param>
    /// <param name="targetId">目标id</param>
    public void SendAttack(int skillId, int attackId, int targetId)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.SkillId, skillId);
        data.Add((byte)ParameterCode.AttackId, attackId);
        data.Add((byte)ParameterCode.TargetId, targetId);
        SendRequest(data);
    }

    /// <summary>
    /// 攻击的服务器响应
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获得攻击的数据
        int fromId = (int)response[(byte)ParameterCode.AttackId];
        int toId = (int)response[(byte)ParameterCode.TargetId];
        int skillId = (int)response[(byte)ParameterCode.SkillId];

        BattleData.Instance.OnAttack(fromId, toId, skillId);
    }
}

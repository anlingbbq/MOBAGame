using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using LitJson;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    /// <summary>
    /// 计算伤害的请求
    /// </summary>
    private DamageRequest m_DamageRequest;

    void Start()
    {
        // 释放资源
        ResourcesManager.Instance.ReleaseAll();

        // 初始化请求
        m_DamageRequest = GetComponent<DamageRequest>();

        // 发送进入游戏的请求
        GetComponent<EnterBattleRequest>().SendRequest();
    }

    /// <summary>
    /// 请求计算攻击伤害
    /// </summary>
    /// <param name="targetId"></param>
    public void RequestDamage(int targetId)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        // 攻击者的id
        data.Add((byte)ParameterCode.AttackId, GameData.Player.Id);
        // 添加技能的id
        data.Add((byte)ParameterCode.SkillId, ServerConfig.SkillId);
        // 添加目标id的数组
        data.Add((byte)ParameterCode.TargetArray, JsonMapper.ToJson(new int[] { targetId }));
        m_DamageRequest.SendRequest(data);
    }
}

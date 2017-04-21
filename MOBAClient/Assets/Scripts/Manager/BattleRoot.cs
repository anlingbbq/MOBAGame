using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using LitJson;
using UnityEngine;

public class BattleRoot : Singleton<BattleRoot>
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
        m_DamageRequest.SendDamage(GameData.Player.Id, ServerConfig.SkillId, targetId);
    }
}

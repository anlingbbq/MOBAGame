using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using LitJson;
using UnityEngine;

namespace MOBAClient
{
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
        /// <param name="attackId">攻击者id</param>
        /// <param name="skillId">技能id</param>
        /// <param name="targetId">目标id</param>
        public void RequestDamage(int attackId, int skillId, int targetId)
        {
            m_DamageRequest.SendDamage(attackId, skillId, targetId);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCtrl : BaseCtrl
{
    public override void Attack()
    {
        // 只发送自己的攻击
        if (this != GameData.HeroCtrl)
            return;

        // 获取目标id
        int targetId = m_Target.GetComponent<BaseCtrl>().Model.Id;
        // 请求计算伤害
        BattleManager.Instance.RequestDamage(targetId);
    }

    /// <summary>
    /// 同步攻击动画
    /// 不计算伤害
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(Transform[] target)
    {
        // 保存目标
        m_Target = target[0];
    }

    protected override void Update()
    {
        base.Update();

        if (m_Target != null)
        {
            // 获取与目标的距离
            float distance = Vector2.Distance(transform.position, m_Target.position);

            // 超过攻击距离
            if (distance > Model.AttackDistance)
            {
                // 移动至目标位置
                Move(m_Target.position);
            }
            // 直接攻击
            else
            {
                // 停止寻路
                m_Agent.Stop();
                // 面向目标
                transform.LookAt(m_Target);
                // 播放动画
                m_AnimeCtrl.Attack();
            }
        }
    }

    public override void DeathResponse()
    {
    }
}

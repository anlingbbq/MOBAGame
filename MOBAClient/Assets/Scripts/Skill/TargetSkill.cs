using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择目标型的技能
/// </summary>
public class TargetSkill : MonoBehaviour
{
    /// <summary>
    /// 目标
    /// </summary>
    private Transform m_Target;

    /// <summary>
    /// 技能id
    /// </summary>
    private int m_SkillId;

    /// <summary>
    /// 攻击者id
    /// </summary>
    private int m_AttackId;

    /// <summary>
    /// 目标id
    /// </summary>
    private int m_TargetId;

    /// <summary>
    /// 是否需要发送请求
    /// </summary>
    private bool m_Send;

    /// <summary>
    /// 伤害请求
    /// </summary>
    private DamageRequest m_DamageRequest;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="target"></param>
    /// <param name="skillId"></param>
    /// <param name="attackId"></param>
    /// <param name="targetId"></param>
    /// <param name="send"></param>
    public void Init(Transform target, int skillId, int attackId, int targetId, bool send)
    {
        m_Target = target;
        m_SkillId = skillId;
        m_AttackId = attackId;
        m_TargetId = targetId;
        m_Send = send;
    }

    void Update()
    {
        // 检测目标
        if (m_Target == null)
            return;

        // 插值移动
        transform.position = Vector3.Lerp(transform.position, m_Target.position, 0.1f);
        float distance = Vector3.Distance(transform.position, m_Target.position);

        // 当距离小于1时发送计算伤害的消息
        if (distance > 1.0f)
            return;

        if (m_Send)
        {
            // 防止重复发送
            m_Send = false;
            // 发送伤害请求
            MOBAClient.BattleManager.Instance.RequestDamage(m_AttackId, m_SkillId, m_TargetId);
        }
        // 销毁物体
        PoolManager.Instance.HideObjet(gameObject);
    }
}

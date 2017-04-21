using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using UnityEngine;

/// <summary>
/// 防御塔控制
/// 为防止重复发送请求
/// 只有己方防御塔执行逻辑
/// </summary>
public class TowerCtrl : BaseCtrl
{
    /// <summary>
    /// 检测攻击范围内的敌人
    /// </summary>
    [SerializeField]
    private TowerCheck m_Check;

    /// <summary>
    /// 攻击点
    /// </summary>
    [SerializeField]
    private Transform m_AttackPos;

    /// <summary>
    /// 是否是己方队伍
    /// </summary>
    private bool m_IsFriend;

    /// <summary>
    /// 攻击间隔计时
    /// </summary>
    private float m_Timer = 0;

    /// <summary>
    /// 攻击的请求
    /// </summary>
    private AttackRequest m_AttackRequest;

    protected override void Start()
    {
        base.Start();

        // 赋值队伍信息
        m_Check.SetTeam(Model.Team);
        m_IsFriend = GameData.HeroCtrl.Model.Team == Model.Team;
    }

    public override void AttackResponse(params Transform[] target)
    {
        // 生成一个攻击特效
        GameObject go = PoolManager.Instance.GetObject("BulletTower");
        go.transform.position = m_AttackPos.position;
        int targetId = target[0].GetComponent<BaseCtrl>().Model.Id;
        go.GetComponent<TargetSkill>().Init(target[0], ServerConfig.SkillId, Model.Id, targetId, m_IsFriend);
    }

    public override void DeathResponse()
    {
        base.DeathResponse();

        this.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (!m_IsFriend)
            return;

        // 检测目标
        if (m_Target == null)
        {
            m_Timer = 0;
            if (m_Check.EnemyList.Count == 0)
                return;

            m_Target = m_Check.EnemyList[0];
        }
        // 检测死亡
        if (m_Target.Model.CurHp <= 0)
        {
            m_Check.EnemyList.Remove(m_Target);
            m_Target = null;
            return;
        }
        // 检测攻击距离
        float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        if (distance > Model.AttackDistance)
        {
            m_Check.EnemyList.Remove(m_Target);
            m_Target = null;
            return;
        }
        // 开始攻击
        m_Timer += Time.deltaTime;
        if (m_Timer >= Model.AttackInterval)
        {
            m_Timer = 0;
            // 向服务器发起攻击的请求
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.AttackId, Model.Id);
            data.Add((byte)ParameterCode.SkillId, ServerConfig.SkillId);
            data.Add((byte)ParameterCode.TargetId, m_Target.Model.Id);
            m_AttackRequest.SendRequest(data);
        }
    }
}

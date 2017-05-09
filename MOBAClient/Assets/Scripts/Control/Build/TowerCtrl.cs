using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using UnityEngine;

/// <summary>
/// 防御塔控制
/// TODO 为防止重复发送请求，只有己方防御塔执行逻辑（这个处理问题很大，先不管）防御塔控制应该由服务器来做？
/// </summary>
public class TowerCtrl : AIBaseCtrl
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
    public Transform m_AttackPos;

    /// <summary>
    /// 是否是己方队伍
    /// </summary>
    public bool m_IsFriend;

    /// <summary>
    /// 攻击间隔计时
    /// </summary>
    private float m_Timer = 0;

    protected override void Start()
    {
        base.Start();

        // 赋值队伍信息
        m_Check.SetTeam(Model.Team);
        m_IsFriend = GameData.HeroCtrl.Model.Team == Model.Team;
    }

    public override void AttackResponse(params AIBaseCtrl[] target)
    {
        // 生成一个攻击特效
        GameObject go = null;
        if (Model.Team == 1)
            go = PoolManager.Instance.GetObject("BulletOne");
        else 
            go = PoolManager.Instance.GetObject("BulletTwo");

        go.transform.position = m_AttackPos.position;
        // 防止重置位置时产生的粒子
        go.GetComponent<EllipsoidParticleEmitter>().emit = true;
        // 初始化
        int targetId = target[0].Model.Id;
        go.GetComponent<TargetSkill>().Init(target[0].transform, ServerConfig.SkillId, Model.Id, targetId, m_IsFriend);
    }

    public override void DeathResponse()
    {
        base.DeathResponse();

        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!m_IsFriend)
            return;

        // 获取目标
        if (Target == null)
        {
            m_Timer = 0;
            if (m_Check.EnemyList.Count == 0)
                return;

            Target = m_Check.EnemyList[0];
        }
        // 检测死亡
        if (Target.Model.CurHp <= 0)
        {
            m_Check.EnemyList.Remove(Target);
            Target = null;
            return;
        }
        // 检测攻击距离
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        if (distance > Model.AttackDistance)
        {
            Target = null;
            return;
        }
        // 开始攻击
        m_Timer += Time.deltaTime;
        if (m_Timer >= Model.AttackInterval)
        {
            m_Timer = 0;
            // 向服务器发起攻击的请求
            MOBAClient.BattleManager.Instance.RequestUseSkill(ServerConfig.SkillId, Model.Id, Target.Model.Id);
        }
    }
}

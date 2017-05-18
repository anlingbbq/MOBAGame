using System;
using Common.Config;
using Common.Dto;
using UnityEngine;

/// <summary>
/// 防御塔控制
/// TODO 防御塔逻辑问题
/// 为防止重复发送请求，只执行己方防御塔逻辑，这个处理问题很大，只适合1v1的情况
/// 当己方有5人时将会执行5次防御塔的逻辑，听谁的这个问题会很麻烦
/// 防御塔逻辑由服务器来做，能很容易解决这个问题，但服务器没有引擎的api，做起来工作量也大，先不做了
/// </summary>
public class TowerCtrl : AIBaseCtrl, IResourceListener
{
    /// <summary>
    /// 攻击点
    /// </summary>
    [SerializeField]
    public Transform m_AttackPos;

    /// <summary>
    /// 索敌雷达
    /// </summary>
    [SerializeField]
    private AIBaseRadar m_Rader;

    /// <summary>
    /// 是否是己方队伍
    /// </summary>
    public bool m_IsFriend;

    /// <summary>
    /// 攻击间隔计时
    /// </summary>
    private float m_Timer;

    protected override void Start()
    {
        base.Start();

        // 加载音效
        ResourcesManager.Instance.Load(Paths.SOUND_TOWER_ATTACK, typeof(AudioClip), this);
    }

    public override void Init(DtoMinion model, bool friend)
    {
        base.Init(model, friend);
        // 设置小地图头像颜色
        if (friend)
            MiniMapHead.color = Color.blue;
        else
            MiniMapHead.color = Color.red;

        m_IsFriend = friend;

        m_Rader.Open(this);
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

        // 音效
        PlayAudio("attack");
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

        // 寻找目标
        if (Target == null || Target.Model.CurHp <= 0)
        {
            Target = m_Rader.FindEnemy();
            if (Target == null)
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

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.SOUND_TOWER_ATTACK:
                m_ClipDict.Add("attack", asset as AudioClip);
                break;
        }
    }
}

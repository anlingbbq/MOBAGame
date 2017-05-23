//#define TEST

using Common.Config;
using Common.Dto;
using UnityEngine;

/// <summary>
/// 小兵逻辑主要是从起点移动到终点
/// 中途遇到敌人则优先攻击
/// TODO 小兵逻辑问题 详见防御塔逻辑问题
/// </summary>
public class MinionCtrl : AIBaseCtrl, IResourceListener, IPoolReuseable
{
    /// <summary>
    /// 索敌雷达
    /// </summary>
    public AIBaseRadar Radar;

    /// <summary>
    /// 终点
    /// </summary>
    [HideInInspector]
    public Vector3 EndPoint;

    private bool m_IsFriend;

    void Awake()
    {
        // 初始化状态机
        AddState(new MinionIdle());
        AddState(new MinionMove());
        AddState(new MinionAttack());
        AddState(new MinionDead());
    }

    protected override void Start()
    {
        base.Start();
        // 加载音效
        ResourcesManager.Instance.Load(Paths.SOUND_MW_ATTACK, typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.SOUND_MW_DEATH, typeof(AudioClip), this);
    }

    public override void Init(DtoMinion model, bool friend)
    {
        base.Init(model, friend);

        Speed = (float)Model.Speed;
        m_IsFriend = friend;

        // 开启寻路
        SetAgent(true);

        // 开启雷达
        Radar.Open(this);

        if (friend)
        {
            // 设置小地图头像颜色
            MiniMapHead.color = Color.blue;
            // 修改纹理
            GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = Resources.Load<Texture>(Paths.TEXTURE_MINION_BULE);
        }
        else
        {
            // 设置小地图头像颜色
            MiniMapHead.color = Color.red;
            // 修改纹理
            GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = Resources.Load<Texture>(Paths.TEXTURE_MINION_RED);
        }

#if TEST
        return;
#endif

        // 设置终点为对方小兵出生地
        if (model.Team == 1)
            EndPoint = BattleData.Instance.Team2MinionPoint[0].position;
        else
            EndPoint = BattleData.Instance.Team1MinionPoint[0].position;

        ChangeState(AIStateEnum.IDLE);
    }

    public override void AttackRequest()
    {
        PlayAudio("attack");

#if TEST
        if (Target != null && Target.Model.CurHp > 0)
        {
            int damage = Model.Attack - Target.Model.Defense;
            if (damage < 0) damage = 0;
            Target.Model.CurHp -= damage;
            Target.OnHpChange();
            if (Target.Model.CurHp <= 0)
            {
                Target.DeathResponse();
            }
        }
        return;
#endif

        // 只发送己方的伤害请求
        if (!m_IsFriend)
            return;

        if (Target == null)
        {
            ChangeState(AIStateEnum.MOVE);
            return;
        }

        // 获取目标id
        int targetId = Target.GetComponent<AIBaseCtrl>().Model.Id;
        // 请求计算伤害 普通攻击
        MOBAClient.BattleManager.Instance.RequestDamage(Model.Id, ServerConfig.SkillId, targetId);
    }

    public override void DeathResponse()
    {
        base.DeathResponse();
        ChangeState(AIStateEnum.DEAD);
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.SOUND_MW_ATTACK:
                m_ClipDict.Add("attack", asset as AudioClip);
                break;
            case Paths.SOUND_MW_DEATH:
                m_ClipDict.Add("death", asset as AudioClip);
                break;
        }
    }

    public void BeforeGetObject()
    {
        RebirthResponse();
    }

    public void BeforeHideObject()
    {
    }
}

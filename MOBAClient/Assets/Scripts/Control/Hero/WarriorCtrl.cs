//#define TEST

using Common.Config;
using Common.Dto;
using UnityEngine;

public class WarriorCtrl : AIBaseCtrl, IResourceListener
{
    void Awake()
    {
        // 初始化状态机
        AddState(new HeroIdel());
        AddState(new HeroMove());
        AddState(new HeroAttack());
        AddState(new HeroDead());
        ChangeState(AIStateEnum.IDLE);
    }

    protected override void Start()
    {
        base.Start();

        // 加载音效
        ResourcesManager.Instance.Load(Paths.SOUND_WARRIOR_ATTACK, typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.SOUND_WARRIOR_DEATH, typeof(AudioClip), this);
    }

    public override void Init(DtoMinion model, bool friend)
    {
        base.Init(model, friend);

        // 设置移动速度
        Speed = (float)Model.Speed;

#if !TEST
        // 调整角度
        MiniMapHead.transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.rotation = Model.Team == 1 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 1);
#endif
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

        // 只发送自己的攻击
        if (Target == null || this != GameData.HeroCtrl)
            return;

        // 获取目标id
        int targetId = Target.GetComponent<AIBaseCtrl>().Model.Id;
        // 请求计算伤害 普通攻击
        MOBAClient.BattleManager.Instance.RequestDamage(GameData.Player.Id, ServerConfig.SkillId, targetId);
    }

    /// <summary>
    /// 接收攻击响应 保存攻击目标
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(params AIBaseCtrl[] target)
    {
        Target = target[0];
        // 攻击状态
        ChangeState(AIStateEnum.ATTACK);
    }

    /// <summary>
    /// 死亡响应
    /// </summary>
    public override void DeathResponse()
    {
        base.DeathResponse();
        // 死亡状态
        ChangeState(AIStateEnum.DEAD);
    }

    void Update()
    {
        if (MiniMapHead.gameObject.activeSelf)
        {
            // 固定小地图头像角度
            MiniMapHead.transform.rotation = Quaternion.Euler(90, 180, 0);
        }
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.SOUND_WARRIOR_ATTACK:
                m_ClipDict.Add("attack", asset as AudioClip);
                break;
            case Paths.SOUND_WARRIOR_DEATH:
                m_ClipDict.Add("death", asset as AudioClip);
                break;
        }
    }
}
    
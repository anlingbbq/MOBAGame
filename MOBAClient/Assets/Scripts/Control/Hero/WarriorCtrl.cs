using Common.Config;
using UnityEngine;

public class WarriorCtrl : AIBaseCtrl, IResourceListener
{
    void Awake()
    {
        // 初始化状态
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
        ResourcesManager.Instance.Load(Paths.RES_SOUND_BATTLE + "WarriorAttack", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_BATTLE + "WarriorDeath", typeof(AudioClip), this);
    }

    public override void AttackRequest()
    {
        PlayAudio("attack");

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
    public override void AttackResponse(params Transform[] target)
    {
        Target = target[0].GetComponent<AIBaseCtrl>();
        // 攻击状态
        ChangeState(AIStateEnum.ATTACK);
    }

    public override void DeathResponse()
    {
        base.DeathResponse();

        // 停止寻路
        m_Agent.Stop();
        // 死亡状态
        ChangeState(AIStateEnum.DEAD);
    }

    public void OnLoaded(string assetName, object asset, AssetType assetType)
    {
        switch (assetName)
        {
            case Paths.RES_SOUND_BATTLE + "WarriorAttack":
                m_ClipDict.Add("attack", asset as AudioClip);
                break;
            case Paths.RES_SOUND_BATTLE + "WarriorDeath":
                m_ClipDict.Add("death", asset as AudioClip);
                break;
        }
    }
}
    
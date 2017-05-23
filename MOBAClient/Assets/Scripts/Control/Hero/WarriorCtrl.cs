//#define TEST

using Common.Config;
using UnityEngine;

public class WarriorCtrl : HeroCtrl, IResourceListener
{
    protected override void Start()
    {
        base.Start();

        // 加载音效
        ResourcesManager.Instance.Load(Paths.SOUND_WARRIOR_ATTACK, typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.SOUND_WARRIOR_DEATH, typeof(AudioClip), this);
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

        // 去除一次性的攻击特效
        string timeKey = GetBuff(SkillData.AttackDouble);
        if (timeKey == null)
            return;

        // 立即执行恢复攻击的计时器
        TimerManager.Instance.RunTimer(timeKey);

        // 通知服务器效果结束
        MOBAClient.BattleManager.Instance.RequestEffectEnd(timeKey);
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
    
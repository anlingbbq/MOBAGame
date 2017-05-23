using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Config;
using Common.Dto;
using MOBAClient;

/// <summary>
/// 技能效果的委托
/// 所有的效果函数需要按照这个委托实现
/// </summary>
/// <param name="skillId"></param>
/// <param name="level"></param>
/// <param name="from"></param>
/// <param name="to"></param>
/// <param name="effect"></param>
public delegate void EffectHandler(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect);

/// <summary>
/// 保存所有的技能处理函数
/// </summary>
public class SkillHandlerData
{
    /// <summary>
    /// 保存所有的委托
    /// </summary>
    public static Dictionary<string, EffectHandler> HandlerDict;

    /// <summary>
    /// 创建当前游戏中需要的效果委托
    /// </summary>
    public static void Init(DtoHero[] heros)
    {
        HandlerDict = new Dictionary<string, EffectHandler>();
        Type typeData = typeof(SkillHandlerData);
        MethodInfo method = typeData.GetMethod("NormalAttack");
        // 普通攻击
        HandlerDict.Add(SkillData.NormalAttack, 
            (EffectHandler)Delegate.CreateDelegate(typeof(EffectHandler), method));

        #region 根据英雄技能创建效果委托
        foreach (DtoHero hero in heros)
        {
            foreach (DtoSkill skill in hero.Skills)
            {
                if (skill == null)
                    continue;

                SkillModel model = SkillManager.Instance.SkillDict.ExTryGet(skill.Id);
                if (model == null)
                    continue;

                // 技能每等级的效果可能不一样 统统遍历一次
                foreach (SkillLevelModel levelModel in model.LvData)
                {
                    foreach (EffectModel effectModel in levelModel.EffectData)
                    {
                        // 寻找对应函数
                        string name = null;
                        if (effectModel.Type.StartsWith(SkillData.EffectType))
                            name = effectModel.Type.Substring(SkillData.EffectType.Length);
                        else if (effectModel.Type.StartsWith(SkillData.DamageType))
                            name = effectModel.Type.Substring(SkillData.DamageType.Length);
                        method = typeData.GetMethod(name);

                        // 创建委托
                        if (method == null)
                        {
                            Log.Error("没有对应的效果处理函数 ：" + effectModel.Type);
                            continue;
                        }
                        if (!HandlerDict.ContainsKey(effectModel.Type))
                        {
                            HandlerDict.Add(effectModel.Type, (EffectHandler)Delegate.CreateDelegate(typeof(EffectHandler), method));
                        }
                    }
                }
            }
        }
        #endregion
    }

    #region 伤害类

    /// <summary>
    /// 普通攻击
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="level"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public static void NormalAttack(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect)
    {
        if (from == null || to == null)
            return;

        // 调用攻击方法
        from.AttackResponse(to);
    }

    #endregion

    #region 效果类

    /// <summary>
    /// 攻击加倍效果 直到时间结束或进行一次普通攻击
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="level"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public static void AttackDouble(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect)
    {
        // 计时器键值
        string timeKey = ServerConfig.GetEffectKey(from.Model.Id, skillId, effect.Type);
        // 添加buff
        from.AddBuff(effect.Type, timeKey);
        // 增加攻击
        int value = from.Model.Attack * ((int)effect.EffectValue - 1);
        from.Model.Attack += value;
        // 更新ui
        if (from.Model.Id == GameData.HeroData.Id)
            (UIManager.Instance.GetPanel(UIPanelType.Battle) as BattlePanel).UpdateView();

        // 恢复攻击
        TimerManager.Instance.AddTimer(timeKey, (float)effect.Duration, () =>
        {
            from.Model.Attack -= value;
            // 移除buff
            from.RemoveBuff(effect.Type);
            // 更新ui
            if (from.Model.Id == GameData.HeroData.Id)
                (UIManager.Instance.GetPanel(UIPanelType.Battle) as BattlePanel).UpdateView();
        });
    }

    /// <summary>
    /// 移速加倍 直到时间结束
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="level"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public static void SpeedDouble(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect)
    {
        // 计时器键值
        string timeKey = ServerConfig.GetEffectKey(from.Model.Id, skillId, effect.Type);
        // 添加buff
        from.AddBuff(effect.Type, timeKey);
        // 增加移速
        float value = from.Speed * (float)(effect.EffectValue - 1);
        from.Speed += value;
        // 恢复移速
        TimerManager.Instance.AddTimer(timeKey, (float)effect.Duration, () =>
        {
            from.Speed -= value;
            // 移除buff
            from.RemoveBuff(effect.Type);
        });
    }

    public static void AttackSpeedDouble(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect)
    {
        // 计时器键值
        string timeKey = ServerConfig.GetEffectKey(from.Model.Id, skillId, effect.Type);
        // 添加buff
        from.AddBuff(effect.Type, timeKey);
        float original = (float)from.Model.AttackInterval;
        // 增加攻速 1/攻击间隔 转换为每秒攻击次数 再乘以倍数
        from.Model.AttackInterval = 1 / (1 / from.Model.AttackInterval * effect.EffectValue);
        from.AnimeCtrl.SetAttackSpeed((float)from.Model.AttackInterval);
        // 恢复攻速
        TimerManager.Instance.AddTimer(timeKey, (float)effect.Duration, () =>
        {
            from.Model.AttackInterval = original;
            from.AnimeCtrl.SetAttackSpeed(original);
            // 移除buff
            from.RemoveBuff(effect.Type);
        });
    }

    #endregion
}

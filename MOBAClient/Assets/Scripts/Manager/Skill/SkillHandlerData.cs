﻿using System;
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
    public static Dictionary<EffectType, EffectHandler> HandlerDict;

    /// <summary>
    /// 创建当前游戏中需要的效果委托
    /// </summary>
    public static void Init(DtoHero[] heros)
    {
        HandlerDict = new Dictionary<EffectType, EffectHandler>();
        Type dataType = typeof(SkillHandlerData);
        MethodInfo info = null;
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
                        info = dataType.GetMethod(Enum.GetName(typeof(EffectType), effectModel.Type));
                        if (!HandlerDict.ContainsKey(effectModel.Type))
                        {
                            HandlerDict.Add(effectModel.Type, (EffectHandler)Delegate.CreateDelegate(typeof(EffectHandler), info));
                        }
                    }
                }
            }
        }
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
    public static void NormalAttack(int skillId, int level, DtoMinion from, DtoMinion[] to, EffectModel effect)
    {
        
    }

    #endregion

    #region 效果类

    /// <summary>
    /// 攻击buff/debuff
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="level"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public static void AttackDouble(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect)
    {
        // 增加攻击
        int value = from.Model.Attack * ((int)effect.EffectValue - 1);
        from.Model.Attack += value;
        // 恢复攻击
        TimerManager.Instance.AddTimer("" + from.Model.Id + skillId + effect.Type, (float)effect.Duration, () =>
        {
            from.Model.Attack -= value;
        });
    }

    /// <summary>
    /// 移速buff/debuff
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="level"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public static void SpeedDouble(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to, EffectModel effect)
    {
        // 增加移速
        float value = from.Speed * (float)(effect.EffectValue - 1);
        from.Speed += value;
        // 恢复移速
        TimerManager.Instance.AddTimer("" + from.Model.Id + skillId + effect.Type, (float)effect.Duration, () =>
        {
            from.Speed -= value;
        });
    }

    #endregion
}
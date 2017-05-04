using System;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using UnityEngine;

namespace MOBAClient
{
    /// <summary>
    /// 保存技能数据 使技能脱离英雄
    /// 键值为 "" + hero.id + skill.id
    /// </summary>
    public class SkillData : Singleton<SkillData>
    {
        /// <summary>
        /// 保存技能数据
        /// </summary>
        public Dictionary<string, DtoSkill> SkillDict = new Dictionary<string, DtoSkill>();

        /// <summary>
        /// 保存技能处理对象
        /// </summary>
        public Dictionary<string, SkillHandler> HandlerDict = new Dictionary<string, SkillHandler>();

        public void Init(DtoHero[] heros)
        {
            foreach (DtoHero hero in heros)
            {
                foreach (DtoSkill skill in hero.Skills)
                {
                    if (skill == null)
                        continue;

                    string key = "" + hero.Id + skill.Id;
                    SkillDict.Add(key, skill);
                    HandlerDict.Add(key, ParseSkill(key, skill));
                }
            }
        }

        /// <summary>
        /// 分析技能效果 转换为处理对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="skill"></param>
        private SkillHandler ParseSkill(string key, DtoSkill skill)
        {
            if (skill.EffectData == null)
                return null;

            // 创建处理对象
            SkillHandler skillHandler = new SkillHandler(key);
            skillHandler.Effects = new EffectModel[skill.EffectData.Length];
            // 遍历技能效果
            for (int i = 0; i < skill.EffectData.Length; i++)
            {
                EffectModel effect = skill.EffectData[i];
                skillHandler.Effects[i] = effect;
                // 判断效果类型 将相应的处理添加到委托里
                switch (effect.Type)
                {
                    case  EffectType.AttackDouble:
                        skillHandler.Handler += AttackDouble;
                        break;
                    case EffectType.SpeedDouble:
                        skillHandler.Handler += SpeedDouble;
                        break;
                }
            }
            return skillHandler;
        }

        /// <summary>
        /// 升级技能
        /// </summary>
        /// <param name="key"></param>
        /// <param name="skill"></param>
        public bool UpgradeSkill(string key, DtoSkill skill)
        {
            if (HandlerDict.ContainsKey(key))
            {
                SkillDict[key] = skill;
                HandlerDict[key] = ParseSkill(key, skill);
                return true;
            }
            return false;
        }

        #region 技能的处理函数

        /// <summary>
        /// 攻击buff/debuff
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="skillKey"></param>
        /// <param name="effect"></param>
        public void AttackDouble(AIBaseCtrl self, AIBaseCtrl target, string skillKey, EffectModel effect)
        {
            // 增加攻击
            int value = self.Model.Attack * ((int)effect.EffectValue - 1);
            self.Model.Attack += value;
            // 恢复攻击
            TimerManager.Instance.AddTimer(skillKey + effect.Type, (float)effect.Duration, () =>
            {
                self.Model.Attack -= value;
            });
        }

        /// <summary>
        /// 移速buff/debuff
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="skillKey"></param>
        /// <param name="effect"></param>
        public void SpeedDouble(AIBaseCtrl self, AIBaseCtrl target, string skillKey, EffectModel effect)
        {
            // 增加移速
            float value = self.Speed * ((float)effect.EffectValue - 1);
            self.Speed += value;
            // 恢复移速
            TimerManager.Instance.AddTimer(skillKey + effect.Type, (float)effect.Duration, () =>
            {
                self.Speed -= value;
            });
        }

        #endregion

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="id"></param>
        public void RunSkill(string key, AIBaseCtrl self, AIBaseCtrl target = null)
        {
            SkillHandler handler = HandlerDict.ExTryGet(key);
            if (handler != null)
            {
                handler.RunSkill(self, target);
            }
        }
    }

    /// <summary>
    /// 技能处理对象
    /// </summary>
    public class SkillHandler
    {
        /// <summary>
        /// 技能唯一标识
        /// </summary>
        public string SkillKey;
        /// <summary>
        /// 效果数据
        /// </summary>
        public EffectModel[] Effects;
        /// <summary>
        /// 实际的调用方法
        /// 使用委托保存所有的效果处理方式
        /// </summary>
        public Action<AIBaseCtrl, AIBaseCtrl, string, EffectModel> Handler;

        public SkillHandler(string key)
        {
            SkillKey = key;
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="self">使用者</param>
        /// <param name="target">目标</param>
        public void RunSkill(AIBaseCtrl self, AIBaseCtrl target)
        {
            for (int i = 0; i < Handler.GetInvocationList().Length; i++)
            {
                var action = (Action<AIBaseCtrl, AIBaseCtrl, string, EffectModel>)Handler.GetInvocationList()[i];
                action(self, target, SkillKey, Effects[i]);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using UnityEngine;

namespace MOBAClient
{
    /// <summary>
    /// 保存技能数据
    /// 由效果组合技能
    /// </summary>
    public class SkillManager : Singleton<SkillManager>
    {
        /// <summary>
        /// 保存技能各等级的处理对象
        /// </summary>
        public Dictionary<int, SkillHandler[]> HandlerDict = new Dictionary<int, SkillHandler[]>();

        /// <summary>
        /// 保存当前游戏用到的技能模型
        /// </summary>
        public Dictionary<int, SkillModel> SkillDict = new Dictionary<int, SkillModel>();

        public void Init(DtoHero[] heros, SkillModel[] skills)
        {
            // 创建技能模型字典
            foreach (SkillModel model in skills)
            {
                SkillDict.Add(model.Id, model); 
            }

            // 创建所有用到的效果委托
            SkillHandlerData.Init(heros);

            foreach (DtoHero hero in heros)
            {
                foreach (DtoSkill dto in hero.Skills)
                {
                    if (dto == null)
                        continue;

                    if (HandlerDict.ContainsKey(dto.Id))
                        continue;

                    // 寻找对应的技能模型
                    SkillModel model = SkillDict.ExTryGet(dto.Id);
                    if (model == null)
                        continue;

                    HandlerDict.Add(dto.Id, ParseSkill(model));
                }
            }
        }

        /// <summary>
        /// 分析技能效果 转换为处理对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="skill"></param>
        private SkillHandler[] ParseSkill(SkillModel skill)
        {
            // 创建处理对象数组 保存不同的等级
            SkillHandler[] skillHandlers = new SkillHandler[skill.LvData.Length];
            for (int i = 0; i < skillHandlers.Length; i++)
            {
                // 创建每一级的处理对象
                SkillHandler skillHandler = new SkillHandler(skill.Id);
                EffectModel[] effects = skill.LvData[i].EffectData;
                skillHandler.Effects = new EffectModel[effects.Length];
                // 遍历技能效果
                for (int j = 0; j < effects.Length; j++)
                {
                    EffectModel effect = effects[j];
                    skillHandler.Effects[j] = effect;
                    // 判断效果类型 将相应的处理添加到委托里
                    EffectHandler handler = SkillHandlerData.HandlerDict.ExTryGet(effect.Type);
                    if (handler != null)
                    {
                        skillHandler.Handler += handler;
                    }
                }
                skillHandlers[i] = skillHandler;
            }
            return skillHandlers;
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void UseSkill(int skillId, int level, AIBaseCtrl from, AIBaseCtrl[] to = null)
        {
            SkillHandler handler = HandlerDict.ExTryGet(skillId)[level];
            if (handler != null)
            {
                handler.RunSkill(level, from, to);
            }
        }

        protected override void OnDestroy()
        {
            SkillHandlerData.HandlerDict = null;
            base.OnDestroy();
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
        public int Id;
        /// <summary>
        /// 效果数据
        /// </summary>
        public EffectModel[] Effects;
        /// <summary>
        /// 使用委托保存所有的效果处理函数
        /// </summary>
        public EffectHandler Handler;

        public SkillHandler(int id)
        {
            Id = id;
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="level"></param>
        /// <param name="from">使用者</param>
        /// <param name="to">目标</param>
        public void RunSkill(int level, AIBaseCtrl from, AIBaseCtrl[] to)
        {
            for (int i = 0; i < Handler.GetInvocationList().Length; i++)
            {
                var action = (EffectHandler)Handler.GetInvocationList()[i];
                action(Id, level, from, to, Effects[i]);
            }
        }
    }
}

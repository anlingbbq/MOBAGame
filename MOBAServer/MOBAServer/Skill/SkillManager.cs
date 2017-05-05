using System;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using MOBAServer.Extension;
using MOBAServer.Room;

namespace MOBAServer.Skill
{
    /// <summary>
    /// 管理游戏中所有的技能
    /// 通过组合效果的方式创建技能
    /// </summary>
    public class SkillManager
    {
        private static SkillManager _instance;
        public static SkillManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SkillManager();

                return _instance;
            }
        }

        /// <summary>
        /// 保存各等级的技能处理对象
        /// </summary>
        public Dictionary<int, SkillHandler[]> HandlerDict = new Dictionary<int, SkillHandler[]>();

        /// <summary>
        /// 将技能模型转化为处理对象
        /// </summary>
        /// <param name="data"></param>
        public void Init(Dictionary<int, SkillModel> data)
        {
            // 创建所有效果处理的委托
            SkillHandlerData.Init();

            // 根据技能模型寻找对应的处理对象 并保存
            foreach (SkillModel model in data.Values)
            {
                HandlerDict.Add(model.Id, ParseSkill(model));
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
                SkillHandler skillHandler = new SkillHandler();
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
        /// <param name="level"></param>
        /// <param name="room"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>返回伤害数据，要求造成伤害的效果在最后,且只有一个</returns>
        public DtoDamage[] UseSkill(int skillId, int level, DtoMinion from, DtoMinion[] to = null, RoomBase<MobaPeer> room = null)
        {
            DtoDamage[] damages = null;
            SkillHandler skillHandler = HandlerDict.ExTryGet(skillId)[level];
            if (skillHandler != null)
            {
                damages = skillHandler.RunSkill(room, from, to);
            }
            return damages;
        }
    }

    /// <summary>
    /// 技能处理对象
    /// </summary>
    public class SkillHandler
    {
        /// <summary>
        /// 效果数据
        /// </summary>
        public EffectModel[] Effects;
        /// <summary>
        /// 技能的实际调用方法
        /// 使用委托保存所有的效果处理方式
        /// </summary>
        public EffectHandler Handler;

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="room">使用房间</param>
        /// <param name="from">使用者</param>
        /// <param name="to">目标</param>
        /// <returns>返回伤害数据，要求造成伤害的效果在最后,且只有一个</returns>
        public DtoDamage[] RunSkill(RoomBase<MobaPeer>room, DtoMinion from, DtoMinion[] to)
        {
            DtoDamage[] damages = null;
            for (int i = 0; i < Handler.GetInvocationList().Length; i++)
            {
                var action = (EffectHandler)Handler.GetInvocationList()[i];
                damages = action(room, from, to, Effects[i]);
            }
            return damages;
        }
    }
}

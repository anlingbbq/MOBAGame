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
                skillHandler.SkillId = skill.Id;
                EffectModel[] effects = skill.LvData[i].EffectData;
                skillHandler.Data = new EffectModel[effects.Length];
                // 遍历技能效果
                for (int j = 0; j < effects.Length; j++)
                {
                    EffectModel effect = effects[j];
                    skillHandler.Data[j] = effect;
                    // 判断效果类型 将相应的处理添加到委托里
                    if (effect.Type.StartsWith(SkillData.DamageType))
                    {
                        DamageHandler handler = SkillHandlerData.DamageDict.ExTryGet(effect.Type);
                        if (handler != null)
                        {
                            skillHandler.Damage += handler;
                        }
                    }
                    else if (effect.Type.StartsWith(SkillData.EffectType))
                    {
                        EffectHandler handler = SkillHandlerData.EffectDict.ExTryGet(effect.Type);
                        if (handler != null)
                        {
                            skillHandler.Effect += handler;
                        }
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
        /// <returns></returns>
        public void UseSkill(int skillId, int level, DtoMinion from, DtoMinion[] to = null, RoomBase<MobaPeer> room = null)
        {
            SkillHandler skillHandler = HandlerDict.ExTryGet(skillId)[level-1];
            skillHandler.RunSkill(room, from, to);
        }

        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="level"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        public DtoDamage[] Damage(RoomBase<MobaPeer> room, int skillId, int level, DtoMinion from, DtoMinion[] to)
        {
            SkillHandler skillHandler = HandlerDict.ExTryGet(skillId)[level-1];
            return skillHandler.RunDamage(room, from, to);
        }

        /// <summary>
        /// 效果提前结束
        /// </summary>
        /// <param name="room"></param>
        /// <param name="effectType"></param>
        /// <returns></returns>
        public bool EffectEnd(RoomBase<MobaPeer> room, string effectType)
        {
            TimerAction ta = SkillHandlerData.TimerDict.ExTryGet(effectType);
            if (ta == null)
                return false;

            // 移除计时器
            room.Timer.RemoveAction(ta.Id);
            // 调用结束处理
            ta.Action();

            return true;
        }
    }

    /// <summary>
    /// 技能处理对象
    /// </summary>
    public class SkillHandler
    {
        /// <summary>
        /// 技能id
        /// </summary>
        public int SkillId;
        /// <summary>
        /// 效果数据
        /// </summary>
        public EffectModel[] Data;
        /// <summary>
        /// 效果类处理
        /// </summary>
        public EffectHandler Effect;
        /// <summary>
        /// 伤害类处理
        /// </summary>
        public DamageHandler Damage;

        /// <summary>
        /// 使用技能 执行技能效果 不处理伤害
        /// </summary>
        /// <param name="room">使用房间</param>
        /// <param name="from">使用者</param>
        /// <param name="to">目标</param>
        /// <returns></returns>
        public void RunSkill(RoomBase<MobaPeer>room, DtoMinion from, DtoMinion[] to)
        {
            if (Effect == null)
                return;

            for (int i = 0; i < Effect.GetInvocationList().Length; i++)
            {
                var action = (EffectHandler)Effect.GetInvocationList()[i];
                action(room, SkillId, from, to, Data[i]);
            }
        }

        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="room"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public DtoDamage[] RunDamage(RoomBase<MobaPeer> room, DtoMinion from, DtoMinion[] to)
        {
            return Damage == null ? null : Damage(room, SkillId, from, to, Data[Data.Length - 1]);
        }
    }
}

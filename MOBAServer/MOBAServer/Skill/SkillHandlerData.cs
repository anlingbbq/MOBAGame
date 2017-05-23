using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Config;
using Common.Dto;
using MOBAServer.Room;

namespace MOBAServer.Skill
{
    /// <summary>
    /// 效果的处理委托
    /// </summary>
    /// <param name="room"></param>
    /// <param name="skillId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public delegate void EffectHandler(RoomBase<MobaPeer> room, int skillId, DtoMinion from, DtoMinion[] to, EffectModel effect);

    /// <summary>
    /// 伤害的处理委托
    /// </summary>
    /// <param name="room"></param>
    /// /// <param name="skillId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    public delegate DtoDamage[] DamageHandler(RoomBase<MobaPeer> room, int skillId, DtoMinion from, DtoMinion[] to, EffectModel effect);

    /// -----------------------------------------------------------------------------------
    /// <summary>
    /// 保存所有的技能处理函数
    /// 所有新的处理函数添加在这里
    /// 与SkillData中的常量名保持对应
    /// </summary>
    /// -----------------------------------------------------------------------------------
    public class SkillHandlerData
    {
        /// <summary>
        /// 保存效果的委托
        /// </summary>
        public static Dictionary<string, EffectHandler> EffectDict;

        /// <summary>
        /// 保存伤害的委托
        /// </summary>
        public static Dictionary<string, DamageHandler> DamageDict;

        /// <summary>
        /// 保存延时调用的技能效果
        /// 例如：效果提前结束，则将结束的处理保存在这里 随时调用
        /// </summary>
        public static Dictionary<string, TimerAction> TimerDict;

        /// <summary>
        /// 根据SkillData中的静态常量 创建对应委托
        /// </summary>
        public static void Init()
        {
            EffectDict = new Dictionary<string, EffectHandler>();
            DamageDict = new Dictionary<string, DamageHandler>();
            TimerDict  = new Dictionary<string, TimerAction>();

            Type typeHandler = typeof(SkillHandlerData);
            Type typeData = typeof(SkillData);
            MethodInfo method = null;
            // 获取SkillData中的所有静态常量
            foreach (FieldInfo filed in typeData.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                string value = filed.GetValue(null) as string;
                if (value == null)
                    continue;

                // 保存效果类委托
                if (value.StartsWith(SkillData.EffectType))
                {
                    method = typeHandler.GetMethod(filed.Name);
                    if (method == null)
                    {
                        MobaServer.LogWarn(">>>> effect method is not found : " + filed.Name);
                        continue;
                    }
                    EffectDict.Add(value, (EffectHandler)Delegate.CreateDelegate(typeof(EffectHandler), method));
                }
                // 保存伤害类委托
                else if (value.StartsWith(SkillData.DamageType))
                {
                    method = typeHandler.GetMethod(filed.Name);
                    if (method == null)
                    {
                        MobaServer.LogWarn(">>>> damage method is not found : " + filed.Name);
                        continue;
                    }
                    DamageDict.Add(value, (DamageHandler)Delegate.CreateDelegate(typeof(DamageHandler), method));
                }
            }
        }

        #region 伤害类

        /// <summary>
        /// 普通攻击
        /// </summary>
        /// <param name="room"></param>
        /// <param name="skillId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static DtoDamage[] NormalAttack(RoomBase<MobaPeer> room, int skillId, DtoMinion from, DtoMinion[] to, EffectModel effect)
        {
            DtoDamage[] damages = new DtoDamage[to.Length];
            int attack = from.Attack;
            // 遍历被攻击者
            for (int i = 0; i < to.Length; i++)
            {
                DtoMinion item = to[i];
                if (item == null)
                    continue;

                int defense = item.Defense;
                int damage = attack - defense;

                // 减少生命值
                item.CurHp -= damage;
                // 死亡处理
                if (item.CurHp <= 0)
                {
                    ((BattleRoom)room).UnitLost(from, item);
                }

                damages[i] = new DtoDamage(from.Id, item.Id, damage, item.CurHp <= 0);
            }
            return damages;
        }

        #endregion

        #region 效果类

        /// <summary>
        /// 攻击buff/debuff
        /// </summary>
        /// <param name="room"></param>
        /// <param name="skillId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static void AttackDouble(RoomBase<MobaPeer> room, int skillId, DtoMinion from, DtoMinion[] to, 
            EffectModel effect)
        {
            // 增加攻击
            double value = from.Attack * (effect.EffectValue - 1);
            from.Attack += (int)value;
            string timerKey = ServerConfig.GetEffectKey(from.Id, skillId, effect.Type);
            MobaServer.LogInfo(">>>>>>>>>>> value:" + value);
            MobaServer.LogInfo(">>>>>>>>>> attack : " + from.Attack);
            // 恢复攻击
            Action action = () =>
            {
                from.Attack -= (int)value;
                MobaServer.LogInfo(">>>>>>>>>> attack : " + from.Attack);
                // 移除延时处理
                TimerDict.Remove(timerKey);
            };
            Guid guid = room.Timer.AddAction(DateTime.UtcNow.AddSeconds(effect.Duration), action);

            // 添加到延时处理
            TimerDict.Add(timerKey, new TimerAction(guid, action));
        }

        /// <summary>
        /// 移速buff/debuff
        /// </summary>
        /// <param name="room"></param>
        /// <param name="skillId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static void SpeedDouble(RoomBase<MobaPeer> room, int skillId, DtoMinion from, DtoMinion[] to, 
            EffectModel effect)
        {
            // 增加移速
            double value = from.Speed * (effect.EffectValue - 1);
            from.Speed += value;
            // 恢复移速
            room.StartSchedule(DateTime.UtcNow.AddSeconds(effect.Duration), () =>
            {
                from.Speed -= value;
            });
        }

        public static void AttakSpeedDouble(RoomBase<MobaPeer> room, int skillId, DtoMinion from, DtoMinion[] to,
            EffectModel effect)
        {
            double original = from.AttackInterval;
            // 增加攻速 1/攻击间隔 转换为每秒攻击次数 再乘以倍数
            from.AttackInterval = 1 / (1 / from.AttackInterval * (effect.EffectValue - 1));
            // 恢复攻速
            room.StartSchedule(DateTime.UtcNow.AddSeconds(effect.Duration), () =>
            {
                from.AttackInterval = original;
            });
        }

        #endregion
    }

    /// <summary>
    /// 包含Timer中的guid和执行函数
    /// </summary>
    public class TimerAction
    {
        public Guid Id;
        public Action Action;

        public TimerAction(Guid id, Action action)
        {
            Id = id;
            Action = action;
        }
    }
}

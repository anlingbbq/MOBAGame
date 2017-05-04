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
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="effect"></param>
    public delegate DtoDamage[] EffectHandler(RoomBase<MobaPeer> room, DtoMinion from, DtoMinion[] to, EffectModel effect);

    /// <summary>
    /// 保存所有的技能处理函数
    /// </summary>
    public class SkillHandlerData
    {
        /// <summary>
        /// 保存所有的委托
        /// </summary>
        public static Dictionary<EffectType, EffectHandler> HandlerDict;

        public static void Init()
        {
            HandlerDict = new Dictionary<EffectType, EffectHandler>();
            Type dataType = typeof(SkillHandlerData);

            // 遍历技能枚举 寻找对应的函数
            foreach (string name in Enum.GetNames(typeof(EffectType)))
            {
                MethodInfo info = dataType.GetMethod(name);
                if (info == null)
                {
                    MobaServer.LogWarn(">>>> init skill not found : " + name);
                    continue;
                }
                // 保存技能委托
                HandlerDict.Add((EffectType)Enum.Parse(typeof(EffectType), name), (EffectHandler)Delegate.CreateDelegate(typeof(EffectHandler), info));
            }
        }

        #region 伤害类

        /// <summary>
        /// 普通攻击
        /// </summary>
        /// <param name="room"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static DtoDamage[] NormalAttack(RoomBase<MobaPeer> room, DtoMinion from, DtoMinion[] to, EffectModel effect)
        {
            MobaServer.LogInfo(">>>>>>>>> start attack");
            DtoDamage[] damages = new DtoDamage[to.Length];
            int attack = from.Attack;
            // 遍历被攻击者
            for (int i = 0; i < to.Length; i++)
            {
                DtoMinion item = to[i];
                int defense = item.Defense;
                int damage = attack - defense;

                // 减少生命值
                item.CurHp -= damage;
                if (item.CurHp <= 0)
                    item.CurHp = 0;

                damages[i] = new DtoDamage(from.Id, item.Id, damage, item.CurHp == 0);
            }
            return damages;
        }

        #endregion

        #region 效果类

        /// <summary>
        /// 攻击buff/debuff
        /// </summary>
        /// <param name="room"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static DtoDamage[] AttackDouble(RoomBase<MobaPeer> room, DtoMinion from, DtoMinion[] to, EffectModel effect)
        {
            // 增加攻击
            int value = from.Attack * ((int)effect.EffectValue - 1);
            from.Attack += value;
            // 恢复攻击
            room.StartSchedule(DateTime.UtcNow.AddSeconds(effect.Duration), () =>
            {
                from.Attack -= value;
            });
            return null;
        }

        /// <summary>
        /// 移速buff/debuff
        /// </summary>
        /// <param name="room"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static DtoDamage[] SpeedDouble(RoomBase<MobaPeer> room, DtoMinion from, DtoMinion[] to, EffectModel effect)
        {
            // 增加移速
            double value = from.Speed * (effect.EffectValue - 1);
            from.Speed += value;
            // 恢复移速
            room.StartSchedule(DateTime.UtcNow.AddSeconds(effect.Duration), () =>
            {
                from.Speed -= value;
            });
            return null;
        }

        #endregion
    }
}

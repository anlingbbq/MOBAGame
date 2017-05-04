using System.Collections.Generic;

namespace Common.Config
{
    /// ------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 在一个技能里，能造成伤害的效果只能有一个，并且要放到最后
    /// </summary>
    /// ------------------------------------------------------------------------------------------------------------------
    public static class SkillData
    {
        // 保存所有技能模型
        public static Dictionary<int, SkillModel> SkillDict = new Dictionary<int, SkillModel>();

        static SkillData()
        {
            // 创建默认战士技能
            int skillId = HeroData.TypeId_Warrior * ServerConfig.SkillId + 1;

            #region 普通攻击
            CreateSkill(ServerConfig.SkillId, "普攻", "", new SkillLevelModel[]
            {
                new SkillLevelModel(0, 0, 0, 0, new EffectModel[]
                {
                    new EffectModel(EffectType.NormalAttack, 0, 0)
                })
            });
            #endregion

            #region 冲锋技能
            CreateSkill(skillId, "冲锋", "一段时间内,移动速度和下一次攻击加倍", new SkillLevelModel[]
            {
                new SkillLevelModel(1, 6, 0, 0, new EffectModel[]
                {
                    new EffectModel(EffectType.SpeedDouble, 2, 2),
                    new EffectModel(EffectType.AttackDouble, 2, 4),
                }),
                new SkillLevelModel(3, 5, 0, 0, new EffectModel[]
                {
                    new EffectModel(EffectType.SpeedDouble, 2.2, 2.5),
                    new EffectModel(EffectType.AttackDouble, 2, 4),
                }),
                new SkillLevelModel(5, 5, 0, 0, new EffectModel[]
                {
                    new EffectModel(EffectType.SpeedDouble, 3, 2.5),
                    new EffectModel(EffectType.AttackDouble, 2.5, 4),
                }),
                new SkillLevelModel(7, 5, 0, 0, new EffectModel[]
                {
                    new EffectModel(EffectType.SpeedDouble, 3, 2.5),
                    new EffectModel(EffectType.AttackDouble, 5, 4),
                }),
            });
            #endregion
        }

        // 创建技能
        public static void CreateSkill(int id, string name, string desc, SkillLevelModel[] lvData)
        {
            SkillModel skill = new SkillModel(id, name, desc, lvData);
            SkillDict.Add(id, skill);
        }

        /// <summary>
        /// 获取技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SkillModel GetSkill(int id)
        {
            SkillModel skill;
            SkillDict.TryGetValue(id, out skill);

            return skill;
        }
    }

    public class SkillModel
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 技能描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 技能等级数据
        /// </summary>
        public SkillLevelModel[] LvData;

        public SkillModel()
        {
            
        }

        public SkillModel(int id, string name, string desc, SkillLevelModel[] data)
        {
            Id = id;
            Name = name;
            Description = desc;
            LvData = data;
        }
    }

    public class SkillLevelModel
    {
        /// <summary>
        /// 学习等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 冷却时间
        /// </summary>
        public int CoolDown;
    
        /// <summary>
        /// 消耗
        /// </summary>
        public int CostValue;

        /// <summary>
        /// 距离
        /// </summary>
        public double Distance;

        /// <summary>
        /// 效果列表
        /// </summary>
        public EffectModel[] EffectData;


        public SkillLevelModel()
        {
        }

        public SkillLevelModel(int level, int cooldown, int cost, double distance, EffectModel[] data)
        {
            Level = level;
            CoolDown = cooldown;
            CostValue = cost;
            Distance = distance;
            EffectData = data;
        }
    }

    public class EffectModel
    {
        /// <summary>
        /// 效果类型
        /// </summary>
        public EffectType Type;

        /// <summary>
        /// 效果值
        /// </summary>
        public double EffectValue;

        /// <summary>
        /// 持续时间
        /// </summary>
        public double Duration;

        public EffectModel()
        {
            
        }

        public EffectModel(EffectType type, double value, double duration)
        {
            Type = type;
            EffectValue = value;
            Duration = duration;
        }
    }

    public enum EffectType
    {
        /// <summary>
        /// 普通攻击
        /// </summary>
        NormalAttack,
        /// <summary>
        /// 速度加倍
        /// </summary>
        SpeedDouble,
        /// <summary>
        /// 攻击加倍
        /// </summary>
        AttackDouble,
    }
}

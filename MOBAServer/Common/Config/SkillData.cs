using System.Collections.Generic;
using Common.Dto;

namespace Common.Config
{
    /// ------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 在一个技能里，相同等级的伤害效果只能有一个，并且要放到最后
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
                    new EffectModel(NormalAttack, 0, 0)
                })
            });
            #endregion

            #region 冲锋技能
            CreateSkill(skillId, "冲锋", "一段时间内,移动速度和下一次攻击加倍", new SkillLevelModel[]
            {
                new SkillLevelModel(1, 6, 0, 0, new EffectModel[]
                {
                    new EffectModel(SpeedDouble, 2, 2),
                    new EffectModel(AttackDouble, 2, 4),
                }),
                new SkillLevelModel(3, 6, 0, 0, new EffectModel[]
                {
                    new EffectModel(SpeedDouble, 2.2, 2.5),
                    new EffectModel(AttackDouble, 2, 4),
                }),
                new SkillLevelModel(5, 6, 0, 0, new EffectModel[]
                {
                    new EffectModel(SpeedDouble, 3, 2.5),
                    new EffectModel(AttackDouble, 2.5, 4),
                }),
                new SkillLevelModel(7, 6, 0, 0, new EffectModel[]
                {
                    new EffectModel(SpeedDouble, 3, 2.5),
                    new EffectModel(AttackDouble, 4, 4),
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

        /// <summary>
        /// 从英雄数组中获取所有技能模型
        /// </summary>
        /// <returns></returns>
        public static SkillModel[] GetSkillByHeros(DtoHero[] heros)
        {
            // 创建技能模型的字典
            Dictionary<int, SkillModel> dict = new Dictionary<int, SkillModel>();
            // 普通攻击
            SkillModel model;
            SkillDict.TryGetValue(ServerConfig.SkillId, out model);
            dict.Add(ServerConfig.SkillId, model);
            // 遍历英雄技能
            foreach (DtoHero hero in heros)
            {
                foreach (DtoSkill skill in hero.Skills)
                {
                    if (skill == null)
                        continue;

                    SkillDict.TryGetValue(skill.Id, out model);
                    if (model == null || dict.ContainsKey(skill.Id))
                        continue;

                    dict.Add(skill.Id, model);
                }
            }
            // 将字典转为数组
            SkillModel[] array = new SkillModel[dict.Count];
            int i = 0;
            foreach (SkillModel item in dict.Values)
            {
                array[i++] = item;
            }
            return array;
        }

        #region 伤害类型

        /// <summary>
        /// 伤害类型前缀
        /// </summary>
        public const string DamageType = "Damage";
        /// <summary>
        /// 普通攻击
        /// </summary>
        public const string NormalAttack = DamageType + "NormalAttack";

        #endregion

        #region 效果类型

        /// <summary>
        /// 效果类型前缀
        /// </summary>
        public const string EffectType = "Effect";
        /// <summary>
        /// 速度加倍
        /// </summary>
        public const string SpeedDouble = EffectType + "SpeedDouble";
        /// <summary>
        /// 攻击加倍
        /// </summary>
        public const string AttackDouble = EffectType + "AttackDouble";

        #endregion
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
        public string Type;

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

        public EffectModel(string type, double value, double duration)
        {
            Type = type;
            EffectValue = value;
            Duration = duration;
        }
    }
}

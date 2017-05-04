using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Config;

namespace Common.Dto
{
    /// <summary>
    /// 技能数据传输对象
    /// </summary>
    public class DtoSkill
    {
        /// <summary>
        /// 技能id
        /// </summary>
        public int Id;

        /// <summary>
        /// 技能名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 技能描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 消耗值
        /// </summary>
        public int CostValue;

        /// <summary>
        /// 使用距离
        /// </summary>
        public double Distance;

        /// <summary>
        /// 冷却时间
        /// </summary>
        public double CoolDown;

        /// <summary>
        /// 效果列表
        /// </summary>
        public EffectModel[] EffectData;

        /// <summary>
        /// 当前等级
        /// </summary>
        public int Level;

        public DtoSkill()
        {
        }

        public DtoSkill(SkillModel model, int level)
        {
            Id = model.Id;
            Name = model.Name;
            Level = level;
            Description = model.Description;
            if (level <= 0)
                return;
    
            CostValue = model.LvData[level].CostValue;
            Distance = model.LvData[level].Distance;
            CoolDown = model.LvData[level].CoolDown;
            EffectData = model.LvData[level].EffectData;
        }

        /// <summary>
        /// 技能升级
        /// </summary>
        public void Upgrade()
        {
            Level += 1;
            SkillModel model = SkillData.GetSkill(Id);
            CostValue = model.LvData[Level].CostValue;
            Distance = model.LvData[Level].Distance;
            CoolDown = model.LvData[Level].CoolDown;
            EffectData = model.LvData[Level].EffectData;
        }
    }
}

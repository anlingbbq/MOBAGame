using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Dto;

namespace Common.Config.Skill
{
    public interface ISkill
    {
        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="from">攻击者</param>
        /// <param name="to">被攻击者</param>
        /// <param name="level">技能等级</param>
        /// <returns>伤害数据</returns>
        List<DtoDamage> Damge(int level, DtoMinion from, params DtoMinion[] to);
    }
}

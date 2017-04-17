using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Config.Skill
{
    public class DamageData
    {
        // 保存技能id和接口的映射
        static Dictionary<int, ISkill> SkillDict = new Dictionary<int, ISkill>();

        static DamageData()
        {
            // 添加普通攻击的映射
            SkillDict.Add(ServerConfig.SkillId, new NormalAttack());
        }

        /// <summary>
        /// 获取技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ISkill GetSkill(int id)
        {
            if (!SkillDict.ContainsKey(id))
                return null;

            return SkillDict[id];
        }
    }
}

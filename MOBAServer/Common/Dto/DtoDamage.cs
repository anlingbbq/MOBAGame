using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dto
{
    public class DtoDamage
    {
        /// <summary>
        /// 攻击者id
        /// </summary>
        public int FromId;

        /// <summary>
        /// 被攻击者id
        /// </summary>
        public int ToId;

        /// <summary>
        /// 技能id
        /// </summary>
        public int SkillId;

        /// <summary>
        /// 伤害
        /// </summary>
        public int Damage;

        /// <summary>
        /// 是否存活
        /// </summary>
        public bool IsDead;

        public DtoDamage()
        {
            
        }

        public DtoDamage(int fromId, int toId, int damage, bool isDead)
        {
            FromId = fromId;
            ToId = toId;
            Damage = damage;
            IsDead = isDead;
        }

        public override string ToString()
        {
            return "fromId: " + FromId + ", toId : " + ToId + ", Damage : " + Damage;
        }
    }
}

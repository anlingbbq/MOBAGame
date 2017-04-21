using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dto
{
    /// <summary>
    /// 建筑的数据传输对象
    /// </summary>
    public class DtoBuild : DtoMinion
    {
        /// <summary>
        /// 是否攻击
        /// </summary>
        public bool Aggressire { get; set; }

        /// <summary>
        /// 是否重生
        /// </summary>
        public bool Rebirth { get; set; }

        /// <summary>
        /// 重生时间
        /// </summary>
        public int RebirthTime { get; set; }

        public DtoBuild()
        {

        }

        public DtoBuild(int id, int typeId, int team, int maxHp, int attack, int defense,
           double attackDistance, double attackInterval, string name, bool aggressire, bool rebirth, int rebirthTime)
            :base(id, typeId, team, maxHp, attack, defense, attackDistance, attackInterval, name)
        {
            Aggressire = aggressire;
            Rebirth = rebirth;
            RebirthTime = rebirthTime;
        }
    }
}

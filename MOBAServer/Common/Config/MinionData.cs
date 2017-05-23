using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Dto;

namespace Common.Config
{
    /// <summary>
    /// 小兵的数据
    /// 这里小兵不做数据变化，直接用数据传输对象
    /// </summary>
    public class MinionData
    {
        /// <summary>
        /// 近战小兵id
        /// </summary>
        public const int TypeId_Warrior = 0;

        /// <summary>
        /// 保存小兵的数据
        /// </summary>
        public static Dictionary<int, DtoMinion> MinionDict = new Dictionary<int, DtoMinion>();

        static MinionData()
        {
            CreateMinion(TypeId_Warrior, 220, 30, 10, 3, 1.5, 5, "MinionWrrior");
        }

        /// <summary>
        /// 创建原型
        /// </summary>
        public static void CreateMinion(int typeId, int maxHp, int attack, int defense, double attackDistance, 
            double attackInterval, int speed, string name)
        {
            MinionDict.Add(typeId, new DtoMinion(-1, typeId, -1, maxHp, attack, defense, attackDistance, attackInterval, speed, name));
        }

        /// <summary>
        /// 获取原型的复制
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static DtoMinion GetMinionCopy(int id, int typeId)
        {
            DtoMinion prototype;
            MinionDict.TryGetValue(typeId, out prototype);
            if (prototype == null)
                return null;

            return new DtoMinion(id, typeId, prototype.Team, prototype.MaxHp, prototype.Attack, prototype.Defense,
                prototype.AttackDistance, prototype.AttackInterval, prototype.Speed, prototype.Name);
        }
    }
}

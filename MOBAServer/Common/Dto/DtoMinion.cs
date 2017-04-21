using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dto
{
    /// <summary>
    /// 小兵的数据传输对象
    /// 同时也时游戏物体的数据基类
    /// </summary>
    public class DtoMinion
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 队伍
        /// </summary>
        public int Team { get; set; }

        /// <summary>
        /// 当前hp
        /// </summary>
        public int CurHp { get; set; }

        /// <summary>
        /// 最大hp
        /// </summary>
        public int MaxHp { get; set; }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense { get; set; }

        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance { get; set; }

        /// <summary>
        /// 攻击间隔
        /// </summary>
        public double AttackInterval { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        public DtoMinion()
        {
            
        }

        public DtoMinion(int id, int typeId, int team, int maxHp, int attack, int defense, 
            double attackDistance, double attackInterval, string name)
        {
            Id = id;
            TypeId = typeId;
            Team = team;
            CurHp = MaxHp = maxHp;
            Attack = attack;
            Defense = defense;
            AttackDistance = attackDistance;
            AttackInterval = attackInterval;
            Name = name;
        }
    }
}

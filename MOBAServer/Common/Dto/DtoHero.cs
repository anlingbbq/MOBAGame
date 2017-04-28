using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Config;

namespace Common.Dto
{
    /// <summary>
    /// 英雄的数据传输对象
    /// </summary>
    public class DtoHero : DtoMinion
    {
        /// <summary>
        /// 当前mp
        /// </summary>
        public int CurMp { get; set; }

        /// <summary>
        /// 最大mp
        /// </summary>
        public int MaxMp { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 经验值
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// 金币
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// 装备列表
        /// </summary>
        public int[] Equipments { get; set; }

        /// <summary>
        /// 技能列表
        /// </summary>
        public int[] Skills { get; set; }

        /// <summary>
        /// 技能点数
        /// </summary>
        public int SP { get; set; }

        /// <summary>
        /// 击杀
        /// </summary>
        public int Kill { get; set; }

        /// <summary>
        /// 死亡
        /// </summary>
        public int Death { get; set; }

        public DtoHero()
        {
            
        }

        public DtoHero(int id, int typeId, int team, int maxHp, int attack, int defense,
            double attackDistance, double attackInterval, string name, int maxMp, int[] skills)
            :base(id, typeId, team, maxHp, attack, defense, attackDistance, attackInterval, name)
        {
            CurMp = MaxMp = maxMp;
            Level = 1;
            Exp = 0;
            Money = 500;
            Skills = skills;
            SP = 1;
            Kill = 0;
            Death = 0;

            // 初始化装备为-1
            Equipments = new int[ServerConfig.ItemMaxCount];
            for (int i = 0; i < Equipments.Length; i++)
                Equipments[i] = -1;
        }

        // 添加装备
        public void AddItem(ItemModel item)
        {
            for (int i = 0; i < Equipments.Length; i++)
            {
                if (Equipments[i] == -1)
                {
                    // 添加装备属性
                    Equipments[i] = item.Id;
                    Attack += item.Attack;
                    Defense += item.Defense;
                    MaxHp += item.Hp;
                    break;
                }
            }
        }
    }
}

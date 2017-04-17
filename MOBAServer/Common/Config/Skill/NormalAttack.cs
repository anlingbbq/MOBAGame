using System;
using System.Collections.Generic;
using System.Linq;
using Common.Dto;

namespace Common.Config.Skill
{
    public class NormalAttack : ISkill
    {
        public List<DtoDamage> Damge(int level, DtoMinion from, params DtoMinion[] to)
        {
            List<DtoDamage> list = new List<DtoDamage>();

            int attack = from.Attack;
            // 遍历被攻击者
            foreach (DtoMinion item in to)
            {
                int defense = item.Defense;
                int damage = attack - defense;

                // 减少生命值
                item.CurHp -= damage;
                if (item.CurHp <= 0)
                    item.CurHp = 0;

                list.Add(new DtoDamage(from.Id, item.Id, damage, item.CurHp == 0));
            }
            return list;
        }
    }
}

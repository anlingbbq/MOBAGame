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
        /// 技能id
        /// </summary>
        public int[] SkillIds { get; set; }

        /// <summary>
        /// 技能点数
        /// </summary>
        public int SP { get; set; }

        /// <summary>
        /// 技能数据
        /// </summary>
        public DtoSkill[] Skills { get; set; }

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
            double attackDistance, double attackInterval, string name, int maxMp, double speed, int[] skillIds)
            :base(id, typeId, team, maxHp, attack, defense, attackDistance, attackInterval, speed, name)
        {
            CurMp = MaxMp = maxMp;
            Level = 1;
            Exp = 0;
            Money = 500;
            SkillIds = skillIds;
            SP = 1;
            Kill = 0;
            Death = 0;

            // 初始化装备id为-1
            Equipments = new int[ServerConfig.ItemMaxCount];
            for (int i = 0; i < Equipments.Length; i++)
                Equipments[i] = -1;

            // 初始化技能
            Skills = new DtoSkill[SkillIds.Length];
            for (int i = 0; i < SkillIds.Length; i++)
            {
                SkillModel model = SkillData.GetSkill(SkillIds[i]);
                if (model != null)
                {
                    Skills[i] = new DtoSkill(model, 0);
                }
            }
        }

        /// <summary>
        /// 添加装备 
        /// </summary>
        /// <param name="item">装备</param>
        /// <returns>是否成功</returns>
        public bool BuyItem(ItemModel item)
        {
            for (int i = 0; i < Equipments.Length; i++)
            {
                if (Equipments[i] == -1)
                {
                    // 扣钱
                    Money -= item.Price;
                    // 添加装备属性
                    Equipments[i] = item.Id;
                    Attack += item.Attack;
                    Defense += item.Defense;
                    MaxHp += item.Hp;
                    CurHp += item.Hp;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 升级技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public DtoSkill UpgradeSkill(int skillId)
        {
            DtoSkill skill = null;
            foreach (DtoSkill item in Skills)
            {
                if (item.Id == skillId)
                {
                    skill = item;
                    break;
                }
            }
            if (skill == null)
                return null;

            SP -= 1;
            // 升级技能
            skill.Upgrade();
            return skill;
        }

        /// <summary>
        /// 添加经验
        /// </summary>
        /// <param name="exp"></param>
        public void AddExp(int exp)
        {
            Exp += exp;
            if (Exp >= Level*300)
            {
                Exp -= Level*300;
                UpgradeLevel();
            }
        }

        /// <summary>
        /// 英雄升级
        /// </summary>
        private void UpgradeLevel()
        {
            HeroModel model = HeroData.GetHeroData(TypeId);
            Level++;
            SP++;
            CurHp += model.GrowHp;
            MaxHp += model.GrowHp;
            CurMp += model.GrowMp;
            MaxMp += model.GrowMp;
            Attack += model.GrowAttack;
            Defense += model.GrowDefense;
        }
    }
}

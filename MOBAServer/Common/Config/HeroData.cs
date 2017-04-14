using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Config
{
    /// <summary>
    /// 英雄数据
    /// 数据感觉应该从数据库里读
    /// 但是想到还要搞缓存层。。就直接写在这里吧
    /// 英雄typeId从1开始
    /// 技能typeId从1001开始
    /// </summary>
    public class HeroData
    {
        /// <summary>
        /// 战士id
        /// </summary>
        public const int TypeId_Warrior = ServerConfig.HeroTypeId + 1;
        /// <summary>
        /// 弓箭手id
        /// </summary>
        public const int TypeId_Archer = ServerConfig.HeroTypeId + 2;

        /// <summary>
        /// 根据id保存英雄数据
        /// </summary>
        public static Dictionary<int, HeroModel> HeroDict = new Dictionary<int, HeroModel>();

        static HeroData()
        {
            // 创建临时的英雄数据
            createHero(TypeId_Warrior, "战士", 60, 20, 300, 100, 10, 3, 50, 10, 1);
            createHero(TypeId_Archer, "弓箭手", 50, 10, 200, 80, 15, 2, 30, 5, 5);
        }

        /// <summary>
        /// 创建英雄
        /// </summary>
        public static void createHero(int typeId, string name, int baseAttack, int baseDefense, int hp,
            int mp, int growAttack, int growDefense, int growHp, int growMp,
            double attackDistance)
        {
            HeroModel hero = new HeroModel(typeId, name, baseAttack, baseDefense, hp,
                mp, growAttack, growDefense, growHp, growMp,attackDistance);

            HeroDict.Add(hero.TypeId, hero);
        }

        /// <summary>
        /// 通过id获取英雄数据
        /// </summary>
        /// <param name="heroId"></param>
        /// <returns></returns>
        public static HeroModel GetHeroData(int heroId)
        {
            HeroModel hero = null;
            HeroDict.TryGetValue(heroId, out hero);
            return hero;
        }
    }

    /// <summary>
    /// 英雄数据模型
    /// </summary>
    public class HeroModel
    {
        /// <summary>
        /// 英雄编号
        /// </summary>
        public int TypeId;

        /// <summary>
        /// 英雄名字
        /// </summary>
        public string Name;

        /// <summary>
        /// 基础攻击力
        /// </summary>
        public int BaseAttack;

        /// <summary>
        /// 基础防御力
        /// </summary>
        public int BaseDefens;

        /// <summary>
        /// 成长攻击力
        /// </summary>
        public int GrowAttack;

        /// <summary>
        /// 成长防御力
        /// </summary>
        public int GrowDefense;

        /// <summary>
        /// 生命值
        /// </summary>
        public int Hp;

        /// <summary>
        /// 成长生命值
        /// </summary>
        public int GrowHp;

        /// <summary>
        /// 魔法值
        /// </summary>
        public int Mp;

        /// <summary>
        /// 成长魔法值
        /// </summary>
        public int GrowMp;

        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance;

        /// <summary>
        /// 技能id
        /// </summary>
        public int[] SkillIds;

        public HeroModel()
        {
            
        }

        public HeroModel(int typeId, string name, int baseAttack, int baseDefense, int hp,
            int mp, int growAttack, int growDefense, int growHp, int growMp, 
            double attackDistance)
        {
            TypeId = typeId;
            Name = name;
            BaseAttack = baseAttack;
            BaseDefens = baseDefense;
            Hp = hp;
            Mp = mp;
            GrowAttack = growAttack;
            GrowDefense = growDefense;
            GrowHp = growHp;
            GrowMp = growMp;
            AttackDistance = attackDistance;

            SkillIds = new []
            {
                typeId * ServerConfig.SkillId + 1,
                typeId * ServerConfig.SkillId + 2,
                typeId * ServerConfig.SkillId + 3,
                typeId * ServerConfig.SkillId + 4
            };
        }
    }
}

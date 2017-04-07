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
    /// 英雄id从1开始
    /// 技能id从1001开始
    /// </summary>
    public class HeroData
    {
        /// <summary>
        /// 战士id
        /// </summary>
        public const int HeroId_Warrior = 1;
        /// <summary>
        /// 弓箭手id
        /// </summary>
        public const int HeroId_Archer = 2;

        /// <summary>
        /// 根据id保存英雄数据
        /// </summary>
        public static Dictionary<int, HeroModel> HeroDict = new Dictionary<int, HeroModel>();

        static HeroData()
        {
            // 创建临时的英雄数据
            createHero(HeroId_Warrior, "战士", 60, 20, 300, 100, 10, 3, 50, 10, 1,
                new int[] { 1001, 1002, 1003, 1004});

            createHero(HeroId_Archer, "弓箭手", 50, 10, 200, 80, 15, 2, 30, 5, 5,
                new int[] { 2001, 2002, 2003, 2004 });
        }

        /// <summary>
        /// 创建英雄
        /// </summary>
        public static void createHero(int id, string name, int baseAttack, int baseDefense, int hp,
            int mp, int growAttack, int growDefense, int growHp, int growMp,
            double attackDistance, int[] skillIds)
        {
            HeroModel hero = new HeroModel(id, name, baseAttack, baseDefense, hp,
                mp, growAttack, growDefense, growHp, growMp,attackDistance, skillIds);

            HeroDict.Add(hero.Id, hero);
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
        public int Id;

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

        public HeroModel(int id, string name, int baseAttack, int baseDefense, int hp,
            int mp, int growAttack, int growDefense, int growHp, int growMp, 
            double attackDistance, int[] skillIds)
        {
            Id = id;
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
            SkillIds = skillIds;
        }
    }
}

using System.Collections.Generic;

namespace Common.Config
{
    public class BuildData
    {
        /// <summary>
        /// 根据类型id保存模型
        /// </summary>
        private static Dictionary<int, BuildModel> BuildDict = new Dictionary<int, BuildModel>();

        static BuildData()
        {
            CreateBuild(ServerConfig.MainBaseId, 2000, -1, 100, -1, -1, "主基地", false, false, -1);
            CreateBuild(ServerConfig.CampId, 1000, -1, 100, -1, -1, "兵营", false, true, 300);
            CreateBuild(ServerConfig.TowerId, 1200, 20, 20, 15, 2, "炮塔", true, false, -1);
        }

        private static void CreateBuild(int typeId, int hp, int attack, int defense, double attackDistance, 
            double attackInterval, string name, bool agressire, bool rebirth, int rebirthTime)
        {
            BuildModel model = new BuildModel(typeId, hp, attack, defense, attackDistance,
                attackInterval, name, agressire, rebirth, rebirthTime);

            BuildDict.Add(model.TypeId, model);
        }

        /// <summary>
        /// 根据类型获取数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static BuildModel GetBuildData(int typeId)
        {
            BuildModel model;
            BuildDict.TryGetValue(typeId, out model);
            return model;
        }
    }

    public class BuildModel
    {
        /// <summary>
        /// 类型id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// hp
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        /// 攻击
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
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否攻击
        /// </summary>
        public bool Agressire { get; set; }

        /// <summary>
        /// 是否重生
        /// </summary>
        public bool Rebirth { get; set; }

        /// <summary>
        /// 重生时间
        /// </summary>
        public int RebirthTime { get; set; }

        public BuildModel(int typeId, int hp, int attack, int defense, double attackDistance, 
            double attakInterval, string name, bool agressire, bool rebirth, int rebirthTime)
        {
            this.TypeId = typeId;
            this.Hp = hp;
            this.Attack = attack;
            this.Defense = defense;
            this.AttackDistance = attackDistance;
            this.Name = name;
            this.Agressire = agressire;
            this.Rebirth = rebirth;
            this.RebirthTime = rebirthTime;
            this.AttackInterval = attakInterval;
        }
    }
}

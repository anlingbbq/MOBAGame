using System;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;

namespace MOBAServer.Room
{
    public class BattleRoom : RoomBase<MobaPeer>
    {
        #region 队伍1
        // 英雄
        Dictionary<int, DtoHero> teamOneHeros = new Dictionary<int, DtoHero>();
        // 小兵
        Dictionary<int, DtoMinion> teamOneMinions = new Dictionary<int, DtoMinion>();
        // 防御塔
        Dictionary<int, DtoBuild> teamOneBuilds  = new Dictionary<int, DtoBuild>();
        #endregion

        #region 队伍2
        // 英雄
        Dictionary<int, DtoHero> teamTwoHeros = new Dictionary<int, DtoHero>();
        // 小兵
        Dictionary<int, DtoMinion> teamTwoMinions = new Dictionary<int, DtoMinion>();
        // 防御塔
        Dictionary<int, DtoBuild> teamTwoBuilds = new Dictionary<int, DtoBuild>();
        #endregion

        public BattleRoom(int id, int count) : base(id, count)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void Init(List<DtoSelect> team1, List<DtoSelect> team2)
        {
            // 玩家数量
            this.Count = team1.Count + team2.Count;
            // 初始化英雄数据
            foreach (DtoSelect item in team1)
                teamOneHeros.Add(item.PlayerId, GetDtoHero(item, 1));
            foreach (DtoSelect item in team2)
                teamTwoHeros.Add(item.PlayerId, GetDtoHero(item, 2));

            // 初始化建筑
            InitBuild();
        }

        private void InitBuild()
        {
            int teamOneBuildId = ServerConfig.TeamOneBuildId;
            int teamTwoBuildId = ServerConfig.TeamTwoBuildId;
            // 主基地
            teamOneBuilds.Add(teamOneBuildId, GetDtoBuild(teamOneBuildId, ServerConfig.MainBaseId, 1));
            teamOneBuilds.Add(teamTwoBuildId, GetDtoBuild(teamTwoBuildId, ServerConfig.MainBaseId, 2));
            --teamOneBuildId;
            --teamTwoBuildId;
            // 兵营
            teamOneBuilds.Add(teamOneBuildId, GetDtoBuild(teamOneBuildId, ServerConfig.CampId, 1));
            teamOneBuilds.Add(teamTwoBuildId, GetDtoBuild(teamTwoBuildId, ServerConfig.CampId, 2));
            --teamOneBuildId;
            --teamTwoBuildId;
            // 防御塔
            for (int i = 0; i < ServerConfig.TowerCount; i++)
            {
                teamOneBuilds.Add(teamOneBuildId, GetDtoBuild(teamOneBuildId, ServerConfig.TowerId, 1));
                --teamOneBuildId;
            }
            for (int i = 0; i > ServerConfig.TowerCount; i++)
            {
                teamOneBuilds.Add(teamTwoBuildId, GetDtoBuild(teamTwoBuildId, ServerConfig.TowerId, 2));
                --teamTwoBuildId;
            }
        }

        /// <summary>
        /// 小兵id
        /// </summary>
        private int m_MinionId = ServerConfig.MinionId;

        public int MinionId
        {
            get
            {
                m_MinionId--;
                return m_MinionId;
            }
        }

        /// <summary>
        /// 开启定时任务 每30秒生成小兵
        /// </summary>
        private void SpawnMinion()
        {
            this.StartSchedule(DateTime.UtcNow.AddSeconds(30),
                delegate
                {
                    List<DtoMinion> minions = new List<DtoMinion>();

                    // 产生1队小兵
                    DtoMinion minion = new DtoMinion();
                    minion.Id = MinionId;
                    teamOneMinions.Add(minion.Id, minion);
                    minions.Add(minion);

                    // 生产2队小兵
                    minion = new DtoMinion();
                    minion.Id = MinionId;
                    teamTwoMinions.Add(minion.Id, minion);
                    minions.Add(minion);

                    // 告诉客户端出兵了

                    SpawnMinion();
                });
        }

        /// <summary>
        /// 获取英雄数据
        /// </summary>
        /// <param name="dto"></param>
        public DtoHero GetDtoHero(DtoSelect dto, int team)
        {
            // 获取英雄模型
            HeroModel model = HeroData.GetHeroData(dto.HeroId);

            DtoHero hero = new DtoHero(dto.PlayerId, dto.HeroId, team, model.Hp, model.BaseAttack,
                model.BaseDefens, model.AttackDistance, model.Name, model.Mp, model.SkillIds);

            return hero;
        }

        /// <summary>
        /// 获取建筑数据 
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="typeId">类型id</param>
        /// <param name="team">队伍id</param>
        /// <returns></returns>
        public DtoBuild GetDtoBuild(int id, int typeId, int team)
        {
            // 获取建筑模型
            BuildModel model = BuildData.GetBuildData(typeId);
            DtoBuild build = new DtoBuild(id, typeId, team, model.Hp, model.Attack, model.Defense,
                model.AttackDistance, model.Name, model.Agressire, model.Rebirth, model.RebirthTime);

            return build;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Config;
using Common.Dto;

namespace MOBAServer.Room
{
    public class BattleRoom : RoomBase<MobaPeer>
    {
        #region 队伍1数据

        /// <summary>
        /// 英雄
        /// </summary>
        Dictionary<int, DtoHero> TeamOneHeros = new Dictionary<int, DtoHero>();
        /// <summary>
        /// 小兵
        /// </summary>
        Dictionary<int, DtoMinion> TeamOneMinions = new Dictionary<int, DtoMinion>();
        /// <summary>
        /// 建筑
        /// </summary>
        Dictionary<int, DtoBuild> TeamOneBuilds  = new Dictionary<int, DtoBuild>();
       
        #endregion

        #region 队伍2数据

        /// <summary>
        /// 英雄
        /// </summary>
        Dictionary<int, DtoHero> TeamTwoHeros = new Dictionary<int, DtoHero>();
        /// <summary>
        /// 小兵
        /// </summary>
        Dictionary<int, DtoMinion> TeamTwoMinions = new Dictionary<int, DtoMinion>();
        /// <summary>
        /// 建筑
        /// </summary>
        Dictionary<int, DtoBuild> TeamTwoBuilds = new Dictionary<int, DtoBuild>();

        #endregion

        /// <summary>
        /// 获取英雄数组
        /// </summary>
        public DtoHero[] HerosArray
        {
            get
            {
                List<DtoHero> list = new List<DtoHero>();
                list.AddRange(TeamOneHeros.Values);
                list.AddRange(TeamTwoHeros.Values);
                return list.ToArray();
            }
        }
        /// <summary>
        /// 获取建筑数组
        /// </summary>
        public DtoBuild[] BuildsArray
        {
            get
            {
                List<DtoBuild> list = new List<DtoBuild>();
                list.AddRange(TeamOneBuilds.Values);
                list.AddRange(TeamTwoBuilds.Values);
                return list.ToArray();
            }
        }

        /// <summary>
        /// 是否全部进入
        /// </summary>
        public bool IsAllEnter
        {
            get { return PeerList.Count >= Count; }
        }

        /// <summary>
        /// 是否全部离开
        /// </summary>
        public bool IsAllLeave
        {
            get { return PeerList.Count <= 0; }
        }

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
            // 初始化英雄数据
            foreach (DtoSelect item in team1)
                TeamOneHeros.Add(item.PlayerId, GetDtoHero(item, 1));
            foreach (DtoSelect item in team2)
                TeamTwoHeros.Add(item.PlayerId, GetDtoHero(item, 2));

            // 初始化建筑
            // 主基地
            AddBuild(ServerConfig.MainBaseId, 1);
            AddBuild(ServerConfig.MainBaseId, 2);
            // 兵营
            AddBuild(ServerConfig.CampId, 1);
            AddBuild(ServerConfig.CampId, 2);
            // 防御塔
            AddBuild(ServerConfig.TowerId, 1);
            AddBuild(ServerConfig.TowerId, 2);
        }

        /// <summary>
        /// 记录当前建筑id
        /// </summary>
        private int m_CurBuild1Id = ServerConfig.TeamOneBuildId;
        private int m_CurBuild2Id = ServerConfig.TeamTwoBuildId;
        /// <summary>
        /// 添加建筑
        /// </summary>
        /// <param name="type"></param>
        /// <param name="team"></param>
        private void AddBuild(int type, int team)
        {
            if (team == 1)
            {
                TeamOneBuilds.Add(m_CurBuild1Id, GetDtoBuild(m_CurBuild1Id, type, 1));
                --m_CurBuild1Id;
            }
            else if (team == 2)
            {
                TeamOneBuilds.Add(m_CurBuild2Id, GetDtoBuild(m_CurBuild2Id, type, 2));
                --m_CurBuild2Id;
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
                    TeamOneMinions.Add(minion.Id, minion);
                    minions.Add(minion);

                    // 生产2队小兵
                    minion = new DtoMinion();
                    minion.Id = MinionId;
                    TeamTwoMinions.Add(minion.Id, minion);
                    minions.Add(minion);

                    // 告诉客户端出兵了

                    SpawnMinion();
                });
        }

        /// <summary>
        /// 根据选择数据获取英雄数据
        /// </summary>
        /// <param name="dto"></param>
        public DtoHero GetDtoHero(DtoSelect dto, int team)
        {
            // 获取英雄模型
            HeroModel model = HeroData.GetHeroData(dto.HeroId);
            if (model == null)
                return null;

            DtoHero hero = new DtoHero(dto.PlayerId, dto.HeroId, team, model.Hp, model.BaseAttack,
                model.BaseDefens, model.AttackDistance, model.AttackInterval, model.Name, model.Mp, model.SkillIds);

            return hero;
        }

        /// <summary>
        /// 根据标识id获取英雄数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DtoHero GetDtoHero(int id)
        {
            DtoHero hero = null;
            if (TeamOneHeros.TryGetValue(id, out hero))
                return hero;
            if (TeamTwoHeros.TryGetValue(id, out hero))
                return hero;

            return null;
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
                model.AttackDistance, model.AttackInterval, model.Name, model.Agressire, model.Rebirth, model.RebirthTime);

            return build;
        }

        /// <summary>
        /// 根据标识id获取建筑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DtoBuild GetDtoBuild(int id)
        {
            DtoBuild build = null;
            if (TeamOneBuilds.TryGetValue(id, out build))
                return build;
            if (TeamTwoBuilds.TryGetValue(id, out build))
                return build;

            return null;
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="peer"></param>
        public void Enter(MobaPeer peer)
        {
            if (!PeerList.Contains(peer))
                PeerList.Add(peer);
        }

        /// <summary>
        /// 离开的客户端
        /// </summary>
        private List<MobaPeer> LeavePeer = new List<MobaPeer>();
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="peer"></param>
        public void Leave(MobaPeer peer)
        {
            if (PeerList.Contains(peer))
                PeerList.Remove(peer);

            LeavePeer.Add(peer);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            TeamOneHeros.Clear();
            TeamOneBuilds.Clear();
            TeamOneMinions.Clear();

            TeamTwoBuilds.Clear();
            TeamTwoHeros.Clear();
            TeamTwoMinions.Clear();

            PeerList.Clear();
            LeavePeer.Clear();
        }
    }
}

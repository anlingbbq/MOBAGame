using System;
using System.Collections.Generic;
using System.Linq;
using Common.Code;
using Common.Config;
using Common.Dto;
using Common.OpCode;
using LitJson;

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
        /// 记录初始化完成的客户端数量
        /// </summary>
        public int InitCount = 0;
        /// <summary>
        /// 是否全部完成初始化完成
        /// </summary>
        public bool IsAllInit
        {
            get { return InitCount >= Count; }
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

        #region 初始化

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

        /// <summary>
        /// 开启定时任务 每30秒生成小兵
        /// </summary>
        public void SpawnMinion()
        {
            MobaServer.LogInfo("----------- 开始生产小兵");
            List<DtoMinion> minions = new List<DtoMinion>();

            // 生成每队的小兵
            for (int i = 0; i < 3; i++)
            {
                // 产生1队小兵
                DtoMinion minion = MinionData.GetMinionCopy(m_MinionId--, MinionData.TypeId_Warrior);
                minion.Team = 1;
                TeamOneMinions.Add(minion.Id, minion);
                minions.Add(minion);

                // 生产2队小兵
                minion = MinionData.GetMinionCopy(m_MinionId--, MinionData.TypeId_Warrior);
                minion.Team = 2;
                TeamTwoMinions.Add(minion.Id, minion);
                minions.Add(minion);
            }

            // 告诉客户端出兵了
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.MinionArray, JsonMapper.ToJson(minions.ToArray()));
            Brocast(OperationCode.SpawnMinion, data);

            // 等待30秒再次产生小兵
            StartSchedule(DateTime.UtcNow.AddSeconds(30), SpawnMinion);
        }

        #endregion

        #region 获取数据

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
                model.BaseDefens, model.AttackDistance, model.AttackInterval, model.Name, model.Mp, model.Speed, model.SkillIds);

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
        /// 根据标识id获取小兵
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DtoMinion GetDtoMinion(int id)
        {
            DtoMinion minion = null;
            if (TeamOneMinions.TryGetValue(id, out minion))
                return minion;
            if (TeamTwoMinions.TryGetValue(id, out minion))
                return minion;

            return null;
        }

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DtoMinion GetDto(int id)
        {
            DtoMinion dto = null;
            // 英雄的id和玩家数据库id一致 所以正数id是英雄
            if (id >= 0)
            {
                dto = GetDtoHero(id);
            }
            // 建筑id范围
            else if (id >= ServerConfig.TeamTwoBuildId - 100 && id <= ServerConfig.TeamOneBuildId)
            {
                dto = GetDtoBuild(id);
            }
            // 小兵id范围
            else if (id <= ServerConfig.MinionId)
            {
                dto = GetDtoMinion(id);
            }
            if (dto == null)
                MobaServer.LogWarn(">>>>>>>>>>>>>> id:" + id + " cant found");

            return dto;
        }

        /// <summary>
        /// 根据id数组获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public DtoMinion[] GetDtos(int[] ids)
        {
            DtoMinion[] dtos = new DtoMinion[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                dtos[i] = GetDto(ids[i]);
            }
            return dtos;
        }

        #endregion

        /// <summary>
        /// 失去单位
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void UnitLost(DtoMinion from, DtoMinion to)
        {
            // 如果被击杀者是小兵
            if (to.Id <= ServerConfig.MinionId)
            {
                // 移除数据
                if (TeamOneMinions.ContainsKey(to.Id))
                    TeamOneMinions.Remove(to.Id);
                if (TeamTwoMinions.ContainsKey(to.Id))
                    TeamTwoMinions.Remove(to.Id);
            }

            // 如果击杀者是英雄
            if (from.Id > 0)
            {
                // 获得奖励
            }
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

            m_CurBuild1Id = ServerConfig.TeamOneBuildId;
            m_CurBuild2Id = ServerConfig.TeamTwoBuildId;
            m_MinionId = ServerConfig.MinionId;
            InitCount = 0;

            // 移除定时任务
            if (!Guid.Equals(new Guid()))
                Timer.RemoveAction(Guid);
        }
    }
}

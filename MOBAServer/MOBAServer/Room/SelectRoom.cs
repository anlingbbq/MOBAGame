using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Model;

namespace MOBAServer.Room
{
    public class SelectRoom : RoomBase<MobaPeer>
    {
        /// <summary>
        /// 队伍1的玩家id和选择模型
        /// </summary>
        public Dictionary<int, DtoSelect> TeamOneDict;

        /// <summary>
        /// 队伍2的玩家id和选择模型
        /// </summary>
        public Dictionary<int, DtoSelect> TeamTwoDict;

        /// <summary>
        ///  进入的数量
        /// </summary>
        private int EnterCount;

        /// <summary>
        /// 是否全部进入房间
        /// </summary>
        public bool IsAllEnter { get { return EnterCount >= Count; } }

        /// <summary>
        /// 准备的数量
        /// </summary>
        private int ReadyCount;

        /// <summary>
        /// 是否全部准备
        /// </summary>
        public bool IsAllReady
        {
            get { return ReadyCount == Count; }
        }

        public SelectRoom(int id, int count) : base(id, count)
        {
            TeamOneDict = new Dictionary<int, DtoSelect>();
            TeamTwoDict = new Dictionary<int, DtoSelect>();

            EnterCount = 0;
            ReadyCount = 0;
        }

        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void InitRoom(List<int> team1, List<int> team2)
        {   
            // 创建选择模型
            Player player;
            DtoSelect model;
            foreach (int playerId in team1)
            {
                player = Caches.Player.GetPlayer(playerId);
                model = new DtoSelect(playerId, player.Name);
                TeamOneDict.Add(playerId, model);
            }
            foreach (int playerId in team2)
            {
                player = Caches.Player.GetPlayer(playerId);
                model = new DtoSelect(playerId, player.Name);
                TeamTwoDict.Add(playerId, model);
            }
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="peer"></param>
        public void EnterRoom(int playerId, MobaPeer peer)
        {
            if (TeamOneDict.ContainsKey(playerId))
            {
                TeamOneDict[playerId].IsEnter = true;
            }
            else if (TeamTwoDict.ContainsKey(playerId))
            {
                TeamTwoDict[playerId].IsEnter = true;
            }
            else
                return;
            
            // 添加连接对象
            PeerList.Add(peer);
            EnterCount++;
        }

        /// <summary>
        /// 选择英雄
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="heroId"></param>
        public bool Select(int playerId, int heroId)
        {
            // 队友有没有选择相同英雄
            if (TeamOneDict.ContainsKey(playerId))
            {
                foreach (DtoSelect model in TeamOneDict.Values)
                {
                    if (model.HeroId == heroId)
                        return false;
                }
                // 可以选择
                TeamOneDict[playerId].HeroId = heroId;
            }
            else if (TeamTwoDict.ContainsKey(playerId))
            {
                foreach (DtoSelect model in TeamTwoDict.Values)
                {
                    if (model.HeroId == heroId)
                        return false;
                }
                // 可以选择
                TeamTwoDict[playerId].HeroId = heroId;
            }

            return true;
        }

        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool Ready(int playerId)
        {
            if (TeamOneDict.ContainsKey(playerId))
            {
                DtoSelect model = TeamOneDict[playerId];
                // 没有选择的英雄
                if (model.PlayerId == -1)
                    return false;
                model.IsReady = true;
                ReadyCount++;
                return true;
            }
            else if (TeamTwoDict.ContainsKey(playerId))
            {
                DtoSelect model = TeamTwoDict[playerId];
                // 没有选择的英雄
                if (model.PlayerId == -1)
                    return false;
                model.IsReady = true;
                ReadyCount++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="peer"></param>
        public void Leave(MobaPeer peer)
        {
            // 移除退出的客户端连接
            PeerList.Remove(peer);
            // 通知所有其他客户端：有人退出 房间解散 回到主界面
            Brocast(OperationCode.DestroySelect, null);
        }

        /// <summary>
        /// 清空房间数据 
        /// </summary>
        public void Clear()
        {
            TeamOneDict.Clear();
            TeamTwoDict.Clear();
            PeerList.Clear();

            EnterCount = 0;
            ReadyCount = 0;
        }
    }
}

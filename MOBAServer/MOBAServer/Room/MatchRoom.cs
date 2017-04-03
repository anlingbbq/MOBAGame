using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Room
{
    public class MatchRoom : RoomBase<MobaPeer>
    {
        /// <summary>
        /// 队伍1玩家id
        /// </summary>
        public List<int> TeamOneIdList;

        /// <summary>
        /// 队伍2玩家id
        /// </summary>
        public List<int> TeamTwoIdList;

        public MatchRoom(int id, int count) : base(id, count)
        {
            int teamCount = count/2;
            TeamOneIdList = new List<int>(teamCount);
            TeamTwoIdList = new List<int>(teamCount);
        }

        /// <summary>
        /// 房间是否满了
        /// </summary>
        public bool IsFull
        {
            get
            {
                return TeamOneIdList.Capacity == TeamOneIdList.Count &&
                    TeamTwoIdList.Capacity == TeamTwoIdList.Count;
            }
        }

        /// <summary>
        /// 房间是否空了
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return TeamOneIdList.Count == 0 &&
                       TeamTwoIdList.Count == 0;
            }
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool EnterRoom(MobaPeer peer, int playerId)
        {
            // 容量大于数量
            if (TeamOneIdList.Capacity > TeamOneIdList.Count)
            {
                TeamOneIdList.Add(playerId);
                base.EnterRoom(peer);
                return true;
            }
            else if (TeamTwoIdList.Capacity > TeamTwoIdList.Count)
            {
                TeamTwoIdList.Add(playerId);
                base.EnterRoom(peer);
                return true;
            }
            // 房间已满
            return false;
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool LeaveRoom(MobaPeer peer, int playerId)
        {
            if (TeamOneIdList.Contains(playerId))
            {
                TeamOneIdList.Remove(playerId);
                base.LeaveRoom(peer);
                return true;
            }
            else if (TeamTwoIdList.Contains(playerId))
            {
                TeamTwoIdList.Remove(playerId);
                base.LeaveRoom(peer);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清除房间信息
        /// </summary>
        public void Clear()
        {
            TeamOneIdList.Clear();
            TeamTwoIdList.Clear();
            PeerList.Clear();
        }
    }
}

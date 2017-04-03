using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class MatchCache
    {
        #region 缓存房间数据

        /// <summary>
        /// 房间id和房间数据的映射
        /// </summary>
        private Dictionary<int, MatchRoom> m_RoomDict = new Dictionary<int, MatchRoom>();

        /// <summary>
        /// 玩家id和房间id的映射
        /// </summary>
        private Dictionary<int, int> m_PlayerRoomDict = new Dictionary<int, int>();

        /// <summary>
        /// 重用的队列
        /// </summary>
        private Queue<MatchRoom> m_RoomQue = new Queue<MatchRoom>();

        /// <summary>
        /// 主键id
        /// </summary>
        private int Index;

        /// <summary>
        /// 玩家进入匹配队列
        /// 这里只实现1v1的情况
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="peer"></param>
        /// <returns>房间</returns>
        public MatchRoom StartMatch(MobaPeer peer, int playerId)
        {
            // 存在等待的房间
            foreach (MatchRoom item in m_RoomDict.Values)
            {
                if (item.IsFull)
                    continue;

                item.EnterRoom(peer, playerId);
                // 绑定玩家和房间的映射
                m_PlayerRoomDict.Add(playerId, item.Id);
                return item;
            }
            // 不存在等待的房间
            MatchRoom room = null;
            // 有可复用的房间
            if (m_RoomQue.Count > 0)
            {
                // 添加映射
                room = m_RoomQue.Dequeue();
                m_RoomDict.Add(room.Id, room);
                m_PlayerRoomDict.Add(playerId, room.Id);
                // 玩家进入房间
                room.EnterRoom(peer, playerId);
                return room;
            }
            // 没有可复用的房间
            else
            {
                // 这里固定房间只进两个人
                room = new MatchRoom(Index, 2);
                Index++;
                // 添加映射
                m_RoomDict.Add(room.Id, room);
                m_PlayerRoomDict.Add(playerId, room.Id);
                // 玩家进入房间
                room.EnterRoom(peer, playerId);
                return room;
            }
        }

        /// <summary>
        /// 玩家离开匹配队列
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="playerId"></param>
        /// <returns>是否离开成功</returns>
        public bool StopMatch(MobaPeer peer, int playerId)
        {
            // 安全检测
            if (!m_PlayerRoomDict.ContainsKey(playerId))
                return false;

            int roomId = m_PlayerRoomDict[playerId];
            MatchRoom room = null;
            // 检测 防止多线程造成不必要的错误
            if (!m_RoomDict.TryGetValue(roomId, out room))
            {
                return false;
            }

            room.LeaveRoom(peer, playerId);
            m_PlayerRoomDict.Remove(playerId);

            if (room.IsEmpty)
            {
                // 移除映射
                m_RoomDict.Remove(room.Id);
                // 移除定时任务
                if (!room.Guid.Equals(new Guid()))
                    room.Timer.RemoveAction(room.Guid);
                // 清除房间信息
                room.Clear();
                // 回收房间
                m_RoomQue.Enqueue(room);
            }
            return true;
        }

        /// <summary>
        /// 摧毁指定房间
        /// </summary>
        /// <param name="room"></param>
        public void DestoryRoom(MatchRoom room)
        {
            // 移除玩家id和房间id的映射
            foreach (int item in room.TeamOneIdList)
            {
                m_PlayerRoomDict.Remove(item);
            }
            foreach (int item in room.TeamTwoIdList)
            {
                m_PlayerRoomDict.Remove(item);
            }
            // 移除房间id和房间的映射
            m_RoomDict.Remove(room.Id);
            // 清空房间信息
            room.Clear();

            // 回收
            m_RoomQue.Enqueue(room);
        }

        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="playerId"></param>
        public void Offline(MobaPeer peer)
        {
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            StopMatch(peer, playerId);
        }

        #endregion
    }
}

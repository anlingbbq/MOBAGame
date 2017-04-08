using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class MatchCache : RoomCacheBase<MatchRoom>
    {
        #region 缓存房间数据

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
            foreach (MatchRoom item in RoomDict.Values)
            {
                if (item.IsFull)
                    continue;

                item.EnterRoom(peer, playerId);
                // 绑定玩家和房间的映射
                PlayerRoomDict.Add(playerId, item.Id);
                return item;
            }
            // 不存在等待的房间
            MatchRoom room = null;
            // 有可复用的房间
            if (RoomQue.Count > 0)
            {
                // 添加映射
                room = RoomQue.Dequeue();
                RoomDict.Add(room.Id, room);
                PlayerRoomDict.Add(playerId, room.Id);
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
                RoomDict.Add(room.Id, room);
                PlayerRoomDict.Add(playerId, room.Id);
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
            // 获取房间
            MatchRoom room = GetRoomByPlayerId(playerId);
            if (room == null) return false;

            room.LeaveRoom(peer, playerId);
            PlayerRoomDict.Remove(playerId);

            if (room.IsEmpty)
            {
                // 移除映射
                RoomDict.Remove(room.Id);
                // 移除定时任务
                if (!room.Guid.Equals(new Guid()))
                    room.Timer.RemoveAction(room.Guid);
                // 清除房间信息
                room.Clear();
                // 回收房间
                RoomQue.Enqueue(room);
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
                PlayerRoomDict.Remove(item);
            }
            foreach (int item in room.TeamTwoIdList)
            {
                PlayerRoomDict.Remove(item);
            }
            // 移除房间id和房间的映射
            RoomDict.Remove(room.Id);
            // 清空房间信息
            room.Clear();

            // 回收
            RoomQue.Enqueue(room);
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

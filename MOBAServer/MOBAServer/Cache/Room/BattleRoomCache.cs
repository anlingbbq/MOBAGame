using Common.Dto;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using System;
using System.Collections.Generic;

namespace MOBAServer.Cache
{
    public class BattleRoomCache : RoomCacheBase<BattleRoom>
    {
        /// <summary>
        /// 创建战斗的房间
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void CreateRoom(List<DtoSelect> team1, List<DtoSelect> team2)
        {
            BattleRoom room = null;
            if (RoomQue.Count <= 0)
            {
                room = new BattleRoom(Index++, team1.Count + team2.Count);
            }
            else
            {
                room = RoomQue.Dequeue();
            }

            // 初始化房间数据
            room.Init(team1, team2);
            // 添加映射
            foreach (DtoSelect item in team1)
            {
                PlayerRoomDict.Add(item.PlayerId, room.Id);
            }
            foreach (DtoSelect item in team2)
            {
                PlayerRoomDict.Add(item.PlayerId, room.Id);
            }
            RoomDict.Add(room.Id, room);
        }

        public BattleRoom Enter(int playerId, MobaPeer peer)
        {
            BattleRoom room = GetRoomByPlayerId(playerId);
            if (room == null)
                return null;

            room.Enter(peer);
            return room;
        }

        /// <summary>
        /// 摧毁指定房间
        /// </summary>
        /// <param name="room"></param>
        public void DestroyRoom(int roomId)
        {
            BattleRoom room;
            if (!RoomDict.TryGetValue(roomId, out room))
                return;

            // 移除玩家id和房间id的映射
            foreach (DtoHero item in room.HerosArray)
                PlayerRoomDict.Remove(item.Id);

            // 移除房间id和房间的映射
            RoomDict.Remove(roomId);

            // 清空房间内的数据
            room.Clear();
            // 回收
            RoomQue.Enqueue(room);

            MobaServer.LogInfo("战斗房间销毁了");
        }

        /// <summary>
        /// 玩家在战斗房间中下线
        /// </summary>
        /// <param name="peer"></param>
        public void Offline(MobaPeer peer)
        {
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = GetRoomByPlayerId(playerId);
            if (room == null)
                return;

            // 房间处理
            room.Leave(peer);

            // 销毁房间
            if (room.IsAllLeave)
                DestroyRoom(room.Id);
        }
    }
}

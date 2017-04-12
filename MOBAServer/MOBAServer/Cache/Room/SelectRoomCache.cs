using System;
using System.Collections.Generic;
using Common.Config;
using Common.OpCode;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class SelectRoomCache : RoomCacheBase<SelectRoom>
    {
        /// <summary>
        /// 开始选人
        /// </summary>
        public void CreateRoom(List<int> team1, List<int> team2)
        {
            SelectRoom room;
            // 获取房间
            if (RoomQue.Count <= 0)
            {
                room = new SelectRoom(Index++, team1.Count + team2.Count);
            }
            else
            {
                room = RoomQue.Dequeue();
            }
            room.InitRoom(team1, team2);
            // 绑定玩家id和房间id
            foreach (int item in team1)
            {
                PlayerRoomDict.Add(item, room.Id);
            }
            foreach (int item in team2)
            {
                PlayerRoomDict.Add(item, room.Id);
            }
            // 绑定房间id和房间
            RoomDict.Add(room.Id, room);

            // 创建完成 开启定时任务 通知玩家在10s之内进入房间 否则 房间自动销毁
            room.StartSchedule(DateTime.UtcNow.AddSeconds(ServerConfig.SelectRoomTimeOff), () =>
            {
                if (!room.IsAllEnter)
                {
                    // 通知所有玩家房间被销毁了
                    room.Brocast(OperationCode.DestroySelect, null);
                    // 销毁房间
                    DestroyRoom(room.Id);
                }
            });
        }

        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="roomId"></param>
        public void DestroyRoom(int roomId)
        {
            SelectRoom room;
            if (!RoomDict.TryGetValue(roomId, out room))
                return;

            // 移除玩家id和房间id的映射
            foreach (int item in room.TeamOneDict.Keys)
            {
                PlayerRoomDict.Remove(item);
            }
            foreach (int item in room.TeamTwoDict.Keys)
            {
                PlayerRoomDict.Remove(item);
            }
            // 移除房间id和房间的映射
            RoomDict.Remove(roomId);

            // 清空房间内的数据
            room.Clear();

            // 回收
            RoomQue.Enqueue(room);

            MobaServer.LogInfo("选人房间销毁了");
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="peer"></param>
        public SelectRoom EnterRoom(int playerId, MobaPeer peer)
        {
            // 获取房间
            SelectRoom room = GetRoomByPlayerId(playerId);
            if (room == null) return null;

            // 进入房间
            room.EnterRoom(playerId, peer);
            return room;
        }

        /// <summary>
        /// 选择英雄
        /// </summary>
        /// <returns></returns>
        public SelectRoom Select(int playerId, int heroId)
        {
            // 获取房间
            SelectRoom room = GetRoomByPlayerId(playerId);
            if (room == null) return null;

            // 选择英雄
            return room.Select(playerId, heroId) ? room : null;
        }

        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public SelectRoom Ready(int playerId)
        {
            // 获取房间
            SelectRoom room = GetRoomByPlayerId(playerId);

            // 确认选择
            return room.Ready(playerId) ? room : null;
        }

        /// <summary>
        /// 玩家在选人房间中下线
        /// </summary>
        /// <param name="peer"></param>
        public void Offline(MobaPeer peer)
        {
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            SelectRoom room = GetRoomByPlayerId(playerId);
            if (room == null)
                return;

            // 房间处理
            room.Leave(peer);
            // 销毁房间
            DestroyRoom(room.Id);
        }
    }
}

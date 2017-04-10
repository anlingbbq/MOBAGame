using Common.Dto;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using System;
using System.Collections.Generic;
using Common.OpCode;

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
                //room = new BattleRoom();
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

        /// <summary>
        /// 摧毁指定房间
        /// </summary>
        /// <param name="room"></param>
        public void DestroyRoom(BattleRoom room)
        {
           
        }

        /// <summary>
        /// 玩家在战斗房间中下线
        /// </summary>
        /// <param name="peer"></param>
        public void Offline(MobaPeer peer)
        {
            int playerid = UserManager.GetPlayer(peer.Username).Identification;
            int roomId;
            if (!PlayerRoomDict.TryGetValue(playerid, out roomId))
                return;

            BattleRoom room = null;
            if (!RoomDict.TryGetValue(roomId, out room))
                return;

            // 移除退出的客户端连接
            room.PeerList.Remove(peer);
            // 通知所有其他客户端：有人退出战斗
            room.Brocast(OperationCode.DestroySelect, null);
            // 销毁房间
            DestroyRoom(room);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using MOBAServer.Extension;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class SelectCache : RoomCacheBase<SelectRoom>
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
            room.StartSchedule(DateTime.UtcNow.AddSeconds(10), () =>
            {
                // 销毁房间
                if (!room.IsAllEnter)
                {
                    Destroy(room.Id);
                    // 通知所有玩家房间被销毁了
                    room.Brocast(OperationCode.DestroySelect, null, null, 0, "有玩家没有进入房间");
                }
                    
            });
        }

        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="roomId"></param>
        public void Destroy(int roomId)
        {
            SelectRoom room = RoomDict.ExTryGet(roomId);
            if (room == null) return;

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
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="peer"></param>
        public SelectRoom EnterRoom(int playerId, MobaPeer peer)
        {
            int roomId;
            if (!PlayerRoomDict.TryGetValue(playerId, out roomId))
            {
                return null;
            }

            SelectRoom room = RoomDict.ExTryGet(roomId);
            if (room == null) return null;

            room.EnterRoom(playerId, peer);
            return room;
        }
    }
}

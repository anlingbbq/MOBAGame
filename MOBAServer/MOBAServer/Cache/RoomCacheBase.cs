using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class RoomCacheBase<TRoom>
          where TRoom : RoomBase<MobaPeer>
    {
        /// <summary>
        /// 房间id和房间的数据结构
        /// </summary>
        protected Dictionary<int, TRoom> RoomDict = new Dictionary<int, TRoom>();

        /// <summary>
        /// 玩家id和房间id
        /// </summary>
        protected Dictionary<int, int> PlayerRoomDict = new Dictionary<int, int>();

        /// <summary>
        /// 复用的队列
        /// </summary>
        protected Queue<TRoom> RoomQue = new Queue<TRoom>();

        /// <summary>
        /// 主键的id
        /// </summary>
        protected int Index = 0;
    }
}

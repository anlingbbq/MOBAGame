using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler.Match
{
    /// <summary>
    /// 开始匹配
    /// </summary>
    class StartMatchHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理开始匹配的请求");

            // 检测id是否正确
            int playerId = (int) request.Parameters[(byte) ParameterCode.PlayerId];
            if (UserManager.GetPlayer(peer.Username).Identification != playerId)
            {
                MobaServer.LogWarn("非法操作");
                return;
            }
                
            MatchRoom room = Caches.Match.StartMatch(peer, playerId);
            // 如果房间满了 开始选人
            if (room.IsFull)
            {
                // 创建选人的房间
                Caches.Select.CreateRoom(room.TeamOneIdList, room.TeamTwoIdList);

                // 通知所有客户端进入选人的房间
                room.Brocast(OperationCode.StartMatch, null);

                // 摧毁房间
                Caches.Match.DestoryRoom(room);
            }
        }
    }
}

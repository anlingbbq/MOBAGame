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

namespace MOBAServer.Handler.Player
{
    /// <summary>
    /// 开始匹配
    /// </summary>
    class PlayerStartMatchHandler : BaseHandler
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
                // 通知房间内所有人进入选人界面
                room.Brocast(OperationCode.StartMatch, null);
                // 摧毁房间
                Caches.Match.DestoryRoom(room);
            }
        }
    }
}

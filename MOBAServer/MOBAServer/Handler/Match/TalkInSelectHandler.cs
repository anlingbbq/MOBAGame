using System;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class TalkInSelectHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理选人界面的聊天请求");

            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            SelectRoom room = Caches.Select.GetRoomByPlayerId(playerId);
            if (room == null)
            {
                MobaServer.LogError(">>>>> 异常：玩家找不到房间 TalkInSelectHandler");
                return;
            }

            // 将聊天内容发给所有客户端 测试方便
            room.Brocast(OperationCode.TalkInSelect, request.Parameters);
        }
    }
}

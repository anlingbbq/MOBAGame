using System;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class AttackHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("接收攻击请求");

            // 获取自己id
            int playerId = (int) UserManager.GetPlayer(peer.Username).Identification;

            // 获取房间
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
            // 广播转发攻击数据
            room.Brocast(OperationCode.Attack, request.Parameters);
        }
    }
}

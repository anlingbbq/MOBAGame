using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.Dto;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class HeroMoveHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理移动的请求");

            // 获取玩家id
            int playerId = UserManager.GetPlayer(peer.Username).Identification;

            // 获取战斗的房间
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
            if (room == null)
                return;

            // 添加玩家的id 玩家id就是英雄的唯一标识id
            request.Parameters[(byte)ParameterCode.PlayerId] = playerId;
            // 告诉其他客户端：谁移动到哪
            room.Brocast(OpCode, request.Parameters);
        }
    }
}

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

namespace MOBAServer.Handler
{
    public class HeroAttackHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("接收攻击请求");

            // 获取自己id
            int playerId = (int) UserManager.GetPlayer(peer.Username).Identification;

            // 获取房间
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
            // 广播转发攻击数据 添加攻击者的id
            request[(byte)ParameterCode.AttackId] = playerId;
            room.Brocast(OperationCode.HeroAttack, request.Parameters);
        }
    }
}

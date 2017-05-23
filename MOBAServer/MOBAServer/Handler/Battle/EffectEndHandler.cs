using System;
using Common.Code;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using MOBAServer.Skill;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class EffectEndHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
            // 获取结束效果的键值
            string effectKey = request[(byte)ParameterCode.EffectKey] as string;

            // 调用结束效果的处理
            SkillManager.Instance.EffectEnd(room, effectKey);

            // 这里应该还有个广播。。
        }
    }
}

using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class InitCompleteHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo(peer.Username + ":初始化完成");

            BattleRoom room = Caches.Battle.GetRoomByPlayerId(UserManager.GetPlayer(peer.Username).Identification);
            room.InitCount++;
            // 所有客户端初始化完成
            if (room.IsAllInit)
            {
                // 开始生产小兵
                room.SpawnMinion();
            }
        }
    }
}

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
using NHibernate.Criterion;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class BeReadyHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理准备完成的请求");

            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            SelectRoom room = Caches.Select.Ready(playerId);

            if (room == null)
            {
                // 告诉自己确认失败
                SendResponse(peer, request.OperationCode, null, ReturnCode.Falied);
                return;
            }
            // 告诉房间内所有人：有人确定选择
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.PlayerId, playerId);
            room.Brocast(OperationCode.BeReady, data);

            // 如果全部准备好了 则开始战斗
            if (room.IsAllReady)
            {
                // TODO 开始战斗

                // 销毁房间
                Caches.Select.DestroyRoom(room.Id);
            }
        }
    }
}

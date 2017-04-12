using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using NHibernate.Criterion;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class EnterBattleHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理进入战斗房间的请求");

            int plyaerId = (int) UserManager.GetPlayer(peer.Username).Identification;
            MobaServer.LogInfo("PlayerId " + plyaerId);
            BattleRoom room = Caches.Battle.Enter(plyaerId, peer);

            MobaServer.LogWarn("count : " + room.Count + " PeerList : " + room.PeerList.Count);
            // 判断是否全部进入
            if (!room.IsAllEnter)
                return;

            // 发送给所有客户端 英雄和建筑的数据
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.HerosArray, JsonMapper.ToJson(room.HerosArray));
            data.Add((byte)ParameterCode.BuildsArray, JsonMapper.ToJson(room.BuildsArray));
            room.Brocast(OperationCode.EnterBattle, data);
        }
    }
}

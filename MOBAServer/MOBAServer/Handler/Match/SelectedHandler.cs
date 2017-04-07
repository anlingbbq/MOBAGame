using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Extension;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler.Match
{
    class SelectedHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理选择的请求");

            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            int heroId = (int) request.Parameters.ExTryGet((byte) ParameterCode.HeroId);

            SelectRoom room = Caches.Select.Select(playerId, heroId);
            if (room == null)
            {
                // 告诉自己选择失败
                OperationResponse response = new OperationResponse(request.OperationCode);
                response.ReturnCode = (short) ReturnCode.Falied;
                peer.SendOperationResponse(response, sendParameters);
                return;
            }

            // 给房间内的所有人发一条消息：谁选择了什么英雄
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.PlayerId, playerId);
            data.Add((byte)ParameterCode.HeroId, heroId);
            room.Brocast(OperationCode.Selected, data, peer);
        }
    }
}

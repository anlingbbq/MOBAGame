using System;
using Common.Code;
using MOBAServer.Cache;
using Photon.SocketServer;

namespace MOBAServer.Handler.Player
{
    class PlayerStopMatchHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理停止匹配的请求");

            int playerId = (int) request.Parameters[(byte) ParameterCode.PlayerId];

            OperationResponse response = new OperationResponse(request.OperationCode);
            if (Caches.Match.StopMatch(peer, playerId))
            {
                // 告诉客户端离开成功
                response.ReturnCode = (short) ReturnCode.Suceess;
                peer.SendOperationResponse(response, sendParameters);
            }
        }
    }
}

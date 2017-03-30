using System;
using System.Collections.Generic;
using Common.Code;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using Photon.SocketServer;

namespace MOBAServer.Handler.Player
{
    // 获取角色信息
    class PlayerGetInfoHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理获取玩家角色的请求");
            OperationResponse response = new OperationResponse(request.OperationCode);
            
            // 验证连接对象
            if (Caches.User.VerifyPeer(peer))
            {
                // 检测是否存在角色
                if (UserManager.HasPlayer(peer.Username))
                {
                    response.ReturnCode = (short) ReturnCode.Suceess;
                }
                else
                {
                    response.ReturnCode = (short) ReturnCode.Empty;
                }
            }
            else
            {
                response.ReturnCode = (short) ReturnCode.Falied;
                response.DebugMessage = "非法登陆";
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}

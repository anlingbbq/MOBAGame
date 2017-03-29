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
                    // 本来玩家和角色是一对多的关系
                    // 这里简化这个过程 直接选择第一个角色
                    // 反正也没创建第二个角色的界面
                    string dto = JsonMapper.ToJson(UserManager.GetPlayer(peer.Username).ConvertToDot());
                    Dictionary<byte, object> data = new Dictionary<byte, object>();
                    data.Add((byte)ParameterCode.PlayerDot, dto);

                    response.Parameters = data;
                }
                else
                {
                    response.ReturnCode = (short)ReturnCode.Empty;
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

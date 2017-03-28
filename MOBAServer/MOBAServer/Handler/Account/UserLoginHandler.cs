using System;
using Common.Code;
using Photon.SocketServer;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.DataBase.Model;
using MOBAServer.Extension;

namespace MOBAServer.Handler.Account
{
    public class UserLoginHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, 
            SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理登陆请求");
            string username = request.Parameters.ExTryGet((byte)ParameterCode.Username) as string;
            string password = request.Parameters.ExTryGet((byte) ParameterCode.Password) as string;

            // 无效检测
            if (String.IsNullOrEmpty(username)|| String.IsNullOrEmpty(password)) return;

            OperationResponse response = new OperationResponse(request.OperationCode);

            // 验证在线
            if (Caches.User.IsOnLine(username))
            {
                response.ReturnCode = (short) ReturnCode.Falied;
                response.DebugMessage = "账号已登陆";
            }
            else
            {
                // 验证用户名，密码是否正确
                if (UserManager.VerifyUser(username, password))
                {
                    Caches.User.Online(username, peer);
                    response.ReturnCode = (short)ReturnCode.Suceess;
                }
                else
                {
                    response.ReturnCode = (short)ReturnCode.Falied;
                    response.DebugMessage = "用户名或密码不正确";
                }
            }
           
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}

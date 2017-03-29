using System;
using Common.OpCode;
using Photon.SocketServer;
using Common.Code;
using MOBAServer.DataBase.Manager;
using MOBAServer.DataBase.Model;
using MOBAServer.Extension;

namespace MOBAServer.Handler.Account
{
    public class UserRegisterHandler : BaseHandler
    {
        // 处理注册请求
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理注册请求");
            string username = request.Parameters.ExTryGet((byte) ParameterCode.Username) as string;
            string password = request.Parameters.ExTryGet((byte) ParameterCode.Password) as string;

            // 无效检测
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return;

            OperationResponse response = new OperationResponse(request.OperationCode);
            if (UserManager.GetByUsername(username) == null)
            {
                // 添加新用户
                UserManager.Add(new User(username, password));
                response.ReturnCode = (byte) ReturnCode.Suceess;
            }
            else
            {
                response.ReturnCode = (byte) ReturnCode.Falied;
                response.DebugMessage = "账号已存在";
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}

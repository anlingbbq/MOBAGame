using Common.Code;
using Photon.SocketServer;
using Common.OpCode;
using MOBAServer.DataBase.Manager;
using MOBAServer.Extension;

namespace MOBAServer.Handler
{
    public class LoginHandler : BaseHandler
    {
        public LoginHandler()
        {
            OpCode = OperationCode.Login;
        }

        public override void OnOperationRequest(OperationRequest request, 
            SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理登陆请求");
            string username = request.Parameters.ExTryGet((byte)ParameterCode.Username) as string;
            string password = request.Parameters.ExTryGet((byte) ParameterCode.Password) as string;

            bool isExist = UserManager.Instance.VerifyUser(username, password);
            OperationResponse response = new OperationResponse(request.OperationCode);
            if (isExist)
            {
                response.ReturnCode = (short) ReturnCode.Suceess;
            }
            else
            {
                response.ReturnCode = (short) ReturnCode.Falied;
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}

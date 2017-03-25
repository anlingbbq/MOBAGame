using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extension;
using Common.OpCode;
using Photon.SocketServer;
using Common.Code;
using MOBAServer.DataBase.Manager;
using MOBAServer.DataBase.Model;

namespace MOBAServer.Handler
{
    public class RegisterHandler : BaseHandler
    {
        public RegisterHandler()
        {
            OpCode = OperationCode.Register;
        }

        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaClient peer)
        {
            string username = request.Parameters.TryGetEx((byte) ParameterCode.Username) as string;
            string password = request.Parameters.TryGetEx((byte) ParameterCode.Password) as string;

            OperationResponse response = new OperationResponse(request.OperationCode);
            if (UserManager.Instance.GetByUsername(username) == null)
            {
                // 添加新用户
                UserManager.Instance.Add(new User(username, password));
                response.ReturnCode = (byte) ReturnCode.Suceess;
            }
            else
            {
                response.ReturnCode = (byte) ReturnCode.Falied;
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}

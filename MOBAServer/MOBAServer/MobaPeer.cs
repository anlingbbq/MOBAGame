using System.Collections.Generic;
using Common.OpCode;
using MOBAServer.Extension;
using MOBAServer.Handler;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MOBAServer
{
    public class MobaPeer : ClientPeer
    {
        public string Username;

        public MobaPeer(InitRequest initRequest) : base(initRequest)
        {

        }

        // 处理客户端请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = MobaServer.Instance.HandlerDict.ExTryGet((OperationCode) operationRequest.OperationCode);
            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                MobaServer.LogError("找不到请求的对应处理");
            }
        }

        // 客户端断开连接
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if (Username != null)
            {
                MobaServer.LogInfo("客户端断开, Username : " + Username);
            }
            else
            {
                MobaServer.LogInfo("未知客户端断开");
            }
        }

        //public virtual void Send(byte opCode, short retCode, string mess, params object[] parameters)
        //{
        //    OperationResponse response = new OperationResponse();
        //    response.OperationCode = opCode;

        //    response.Parameters = new Dictionary<byte, object>();
        //    for (int i = 0; i < parameters.Length; i++)
        //        response[(byte)i] = parameters[i];

        //    response.ReturnCode = retCode;
        //    response.DebugMessage = mess;

        //    this.SendOperationResponse(response, new SendParameters());
        //}
    }
}

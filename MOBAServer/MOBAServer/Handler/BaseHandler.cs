using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    /// <summary>
    /// 处理消息的基类
    /// </summary>
    public abstract class BaseHandler
    {
        public OperationCode OpCode;

        /// <summary>
        /// 处理接收的客户端请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sendParameters"></param>
        /// <param name="peer"></param>
        public abstract void OnOperationRequest(OperationRequest request,
            SendParameters sendParameters, MobaPeer peer);

        /// <summary>
        /// 发送服务器的响应
        /// </summary>
        /// <param name="peer">接收响应的客户端</param>
        /// <param name="opCode">操作码</param>
        /// <param name="data">发送的数据</param>
        /// <param name="retCode">返回码</param>
        /// <param name="debugMgr">调试信息</param>
        public void SendResponse(MobaPeer peer, byte opCode, Dictionary<byte, object> data = null,
            ReturnCode retCode = ReturnCode.Suceess, string debugMgr = "")
        {
            OperationResponse response = new OperationResponse(opCode);
            response.Parameters = data;
            response.ReturnCode = (short) retCode;
            response.DebugMessage = debugMgr;

            peer.SendOperationResponse(response, new SendParameters());
        }
    }
}

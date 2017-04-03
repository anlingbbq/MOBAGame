using System;
using System.Collections.Generic;
using Common.OpCode;
using MOBAServer.Cache;
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

        /// <summary>
        /// 处理客户端请求 
        /// </summary>
        /// <param name="operationRequest"></param>
        /// <param name="sendParameters"></param>
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = MobaServer.Instance.HandlerDict.ExTryGet((OperationCode) operationRequest.OperationCode);
            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                MobaServer.LogError("找不到请求的对应处理 : " + 
                    Enum.GetName(typeof(OperationCode), operationRequest.OperationCode));
            }
        }

        /// <summary>
        /// 客户端断开连接 
        /// </summary>
        /// <param name="reasonCode"></param>
        /// <param name="reasonDetail"></param>
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if (Username != null)
            {
                // 这里下线的顺序 要从内到外
                Caches.Match.Offline(this);
                Caches.Player.Offline(this);
                Caches.User.Offline(Username);

                MobaServer.LogInfo("客户端断开, Username : " + Username);
            }
            else
            {
                MobaServer.LogInfo("未知客户端断开");
            }
        }
    }
}

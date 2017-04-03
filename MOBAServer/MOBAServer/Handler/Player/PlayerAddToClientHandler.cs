using System;
using System.Collections.Generic;
using System.Linq;
using Common.Code;
using Common.DTO;
using Common.OpCode;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Extension;
using Photon.SocketServer;

namespace MOBAServer.Handler.Player
{
    /// <summary>
    /// 处理客户端是否同意添加好友的反馈
    /// </summary>
    class PlayerAddToClientHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理是否添加好友的反馈");

            // 是否同意
            bool isAccept = (bool)request.Parameters.ExTryGet((byte) ParameterCode.AcceptAddFriend);
            // 请求添加的玩家数据
            int fromId = (int) request.Parameters.ExTryGet((byte) ParameterCode.PlayerId);
            string fromName = (string)request.Parameters.ExTryGet((byte)ParameterCode.PlayerName);

            // 请求的客户端
            MobaPeer fromPeer = Caches.Player.GetPeer(fromId);

            OperationResponse response = new OperationResponse((byte)OperationCode.PlayerAddResult);
            if (isAccept)
            {
                response.ReturnCode = (short) ReturnCode.Suceess;

                // 自身的数据
                DataBase.Model.Player player = UserManager.GetPlayer(peer.Username);
                // 好友的数据
                DataBase.Model.Player friend = PlayerManager.GetById(fromId);

                // 添加好友
                PlayerManager.AddFriend(player.Identification, fromId);

                // 发送更新后的数据
                DtoPlayer dtoPlayer = player.ConvertToDot();
                response.Parameters = new Dictionary<byte, object>();
                response[(byte)ParameterCode.DtoFriend] = JsonMapper.ToJson(dtoPlayer.Friends.Last());
                peer.SendOperationResponse(response, sendParameters);

                dtoPlayer = friend.ConvertToDot();
                response.Parameters = new Dictionary<byte, object>();
                response[(byte)ParameterCode.DtoFriend] = JsonMapper.ToJson(dtoPlayer.Friends.Last());
                fromPeer.SendOperationResponse(response, sendParameters);

                return;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Falied;
                response.DebugMessage = fromName + " 拒绝添加你为好友";
            }

            fromPeer.SendOperationResponse(response, sendParameters);
        }
    }
}

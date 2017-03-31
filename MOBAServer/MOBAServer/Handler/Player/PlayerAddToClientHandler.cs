using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.DTO;
using Common.OpCode;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Extension;
using NHibernate.Criterion;
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
            // 是否同意
            bool isAccept = (bool)request.Parameters.ExTryGet((byte) ParameterCode.AcceptAddFriend);
            // 请求添加的玩家id
            DtoPlayer dtoPlayer = (DtoPlayer)request.Parameters.ExTryGet((byte) ParameterCode.DtoPlayer);

            OperationResponse response = new OperationResponse();
            if (isAccept)
            {
                response.ReturnCode = (short) ReturnCode.Suceess;

                // 添加自身数据的好友列表
                DataBase.Model.Player player = UserManager.GetPlayer(peer.Username);
                if (String.IsNullOrEmpty(player.FriendIdList))
                    player.FriendIdList = dtoPlayer.Name;
                else
                    player.FriendIdList += ',' + dtoPlayer.Name;
                PlayerManager.Update(player);

                // todo 发送新的玩家数据 刷新自身的ui
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Falied;
                response.DebugMessage = dtoPlayer.Name + ":拒绝了添加你为好友";
            }

            MobaPeer fromPeer = Caches.Player.GetPeer(dtoPlayer.Id);
            fromPeer.SendOperationResponse(response, sendParameters);
        }
    }
}

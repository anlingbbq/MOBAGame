using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Extension;
using Photon.SocketServer;

namespace MOBAServer.Handler.Player
{
    /// <summary>
    /// 处理客户端发送添加好友的请求
    /// 成功后会发送响应给要添加的好友
    /// </summary>
    class PlayerAddRequestHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理添加好友的请求");

            // 请求添加的玩家名字
            string name = request.Parameters.ExTryGet((byte) ParameterCode.PlayerName) as string;

            // 无效判断
            if (String.IsNullOrEmpty(name))
                return;

            // 获取添加好友的数据
            OperationResponse response = new OperationResponse(request.OperationCode);
            DataBase.Model.Player player = PlayerManager.GetByName(name);
            if (player == null)
            {
                response.ReturnCode = (short)ReturnCode.Falied;
                response.DebugMessage = "没有此玩家";
                peer.SendOperationResponse(response, sendParameters);
                return;
            }
            // 如果添加的玩家为自身
            DataBase.Model.Player selfPlayer = UserManager.GetPlayer(peer.Username);
            if (selfPlayer.Identification == player.Identification)
            {
                response.ReturnCode = (short)ReturnCode.Falied;
                response.DebugMessage = "不能添加自身";
                peer.SendOperationResponse(response, sendParameters);
                return;
            }
            // 如果已经是好友
            string[] friends = UserManager.GetPlayer(peer.Username).FriendIdList.Split(',');
            foreach (string friend in friends)
            {
                if (String.IsNullOrEmpty(friend))
                    continue;
                if (int.Parse(friend) == player.Identification)
                {
                    response.ReturnCode = (short)ReturnCode.Falied;
                    response.DebugMessage = "玩家已经是你的好友";
                    peer.SendOperationResponse(response, sendParameters);
                    return;
                }
            }
            // 判断是否在线 这里不提供离线处理 直接返回失败
            // TODO 添加好友时玩家离线的功能
            bool isOnline = Caches.Player.IsOnline(player.Identification);
            if (!isOnline)
            {
                response.ReturnCode = (short)ReturnCode.Falied;
                response.DebugMessage = "玩家离线";
                peer.SendOperationResponse(response, sendParameters);
                return;
            }
            // 玩家在线 向玩家发送是否同意的响应
            MobaPeer toPeer = Caches.Player.GetPeer(player.Identification);
            response.OperationCode = (byte)OperationCode.PlayerAddToClient;
            response.Parameters = new Dictionary<byte, object>();
            response[(byte) (ParameterCode.DtoPlayer)] = JsonMapper.ToJson(selfPlayer.ConvertToDot());
            toPeer.SendOperationResponse(response, sendParameters);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using MOBAServer.DataBase.Manager;
using MOBAServer.DataBase.Model;
using MOBAServer.Extension;
using Photon.SocketServer;

namespace MOBAServer.Handler.Player
{
    class PlayerCreateHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理创建角色的请求");

            string playerName = request.Parameters.ExTryGet((byte)ParameterCode.PlayerName) as string;
            // 无效检测
            if (String.IsNullOrEmpty(playerName)) return;

            User user = UserManager.GetByUsername(peer.Username);
            DataBase.Model.Player player = new DataBase.Model.Player(playerName, user);
            PlayerManager.Add(player);

            user.PlayerList.Add(player);
            UserManager.Update(user);

            OperationResponse response = new OperationResponse(request.OperationCode);
            response.ReturnCode = (short) ReturnCode.Suceess;
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}

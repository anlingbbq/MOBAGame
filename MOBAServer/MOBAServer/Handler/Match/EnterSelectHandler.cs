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
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler.Match
{
    public class EnterSelectHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理进入选择的请求");

            DataBase.Model.Player player = UserManager.GetPlayer(peer.Username);
            SelectRoom room = Caches.Select.EnterRoom(player.Identification, peer);
            if (room == null) return;

            OperationResponse response = new OperationResponse();
            // 先给客户端发送已经进入房间的玩家数据(房间模型)
            response.OperationCode = (byte) OperationCode.SelectGetInfo;
            response.Parameters = new Dictionary<byte, object>();
            response[(byte) ParameterCode.TeamOneData] = JsonMapper.ToJson(room.TeamOneDict.ToArray());
            response[(byte) ParameterCode.TeamTwoData] = JsonMapper.ToJson(room.TeamTwoDict.ToArray());
            peer.SendOperationResponse(response, sendParameters);

            // 再给房间内的其他客户端发一条消息:有人进入了
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)(ParameterCode.PlayerId), player.Identification);
            data.Add((byte)(ParameterCode.PlayerName), player.Name);
            room.Brocast(OperationCode.EnterSelect, data);
        }
    }
}

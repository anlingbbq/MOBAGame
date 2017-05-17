using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.Config;
using Common.Dto;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class UpgradeSkillHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理技能升级的请求");

            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);

            // 获取英雄数据
            DtoHero hero = room.GetDtoHero(playerId);
            if (hero == null || hero.SP < 1)
                return;

            // 获取技能id
            int skillId = (int)request[(byte) ParameterCode.SkillId];
            
            // 技能升级
            DtoSkill skill = hero.UpgradeSkill(skillId);
            if (skill == null)
                return;

            // 发送升级的技能数据和英雄id
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.DtoSkill, JsonMapper.ToJson(skill));
            data.Add((byte)ParameterCode.PlayerId, playerId);
            SendResponse(peer, request.OperationCode, data);
        }
    }
}

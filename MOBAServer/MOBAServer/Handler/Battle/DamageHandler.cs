using System;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using Common.Dto;
using Common.OpCode;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using MOBAServer.Skill;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    /// <summary>
    /// 伤害计算的处理
    /// 需要区分攻击者的类型
    /// </summary>
    public class DamageHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            //MobaServer.LogInfo("处理计算伤害的请求");

            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
         
            // 获取技能id
            int skillId = (int) request[(byte)ParameterCode.SkillId];

            // 获取攻击者数据
            int fromId = (int)request[(byte)ParameterCode.FromId];
            DtoMinion from = room.GetDto(fromId);
            if (from == null)
                return;

            // 获取被攻击者的数据
            int[] toIds = JsonMapper.ToObject<int[]>(request[(byte)ParameterCode.TargetArray] as string);
            DtoMinion[] to = room.GetDtos(toIds);

            // 使用技能 获取伤害数据
            DtoDamage[] damages = null;
            damages = SkillManager.Instance.Damage(room, skillId, 1, from, to);

            // 广播伤害数据传输对象
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.DtoDamages, JsonMapper.ToJson(damages));
            room.Brocast(OperationCode.Damage, data);
        }
    }
}

using Common.Code;
using Common.Dto;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using MOBAServer.Skill;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class UseSkillHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            //MobaServer.LogInfo("处理使用技能的请求");

            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);

            // 获取使用者数据
            int fromId = (int)request[(byte)ParameterCode.FromId];
            DtoMinion from = room.GetDto(fromId);

            // 获取目标数据
            DtoMinion[] to = null;
            if (request.Parameters.ContainsKey((byte)ParameterCode.TargetArray))
            {
                int[] toIds = JsonMapper.ToObject<int[]>(request[(byte)ParameterCode.TargetArray] as string);
                to = room.GetDtos(toIds);
            }

            // 获取技能id和等级
            int skillId = (int)request[(byte)ParameterCode.SkillId];
            int level = (int)request[(byte)ParameterCode.SkillLevel];
            // 使用技能
            SkillManager.Instance.UseSkill(skillId, level, from, to, room);
            // 广播谁使用了技能
            room.Brocast(OpCode, request.Parameters);
        }
    }
}

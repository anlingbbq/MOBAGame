using System.Collections.Generic;
using Common.Code;
using Common.Config;
using Common.Dto;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class UserSkillHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            MobaServer.LogInfo("处理使用技能的请求");

            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);

            // 获取技能id
            int skillId = (int)request[(byte)ParameterCode.DtoSkill];
        }

        public void ParseSkill(DtoSkill skill, DtoMinion from, DtoMinion[] to)
        {
            // 遍历技能效果
            for (int i = 0; i < skill.EffectData.Length; i++)
            {
                EffectModel effect = skill.EffectData[i];
                // 判断效果类型
                switch (effect.Type)
                {
                    case EffectType.AttackDouble:

                        break;
                    case EffectType.SpeedDouble:
                        break;
                }
            }
        }
    }
}

using System.Collections.Generic;
using Common.Code;
using Common.Config;
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
            MobaServer.LogInfo(">>>> 处理使用技能的请求");

            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);

            #region 获取使用者数据

            int fromId = (int)request[(byte)ParameterCode.FromId];
            DtoMinion from = null;
            // 英雄的id和玩家数据库id一致 所以正数id是英雄
            if (fromId >= 0)
            {
                from = room.GetDtoHero(fromId);

            }
            // 建筑id范围
            else if (fromId >= ServerConfig.TeamTwoBuildId - 100 && fromId <= ServerConfig.TeamOneBuildId)
            {
                from = room.GetDtoBuild(fromId);
            }
            // 小兵id范围
            else if (fromId <= ServerConfig.MinionId)
            {

            }

            #endregion

            #region 获取目标数据

            DtoMinion[] to = null;
            if (request.Parameters.ContainsKey((byte) ParameterCode.TargetArray))
            {
                int[] toIds = JsonMapper.ToObject<int[]>(request[(byte)ParameterCode.TargetArray] as string);
                to = new DtoMinion[toIds.Length];
                for (int i = 0; i < toIds.Length; i++)
                {
                    // 英雄的id和玩家数据库id一致 所以正数id是英雄
                    if (toIds[i] >= 0)
                    {
                        to[i] = room.GetDtoHero(toIds[i]);

                    }
                    // 建筑id范围
                    else if (toIds[i] >= ServerConfig.TeamTwoBuildId - 100 && toIds[i] <= ServerConfig.TeamOneBuildId)
                    {
                        to[i] = room.GetDtoBuild(toIds[i]);
                    }
                    // 小兵id范围
                    else if (toIds[i] <= ServerConfig.MinionId)
                    {

                    }
                }
            }
          

            #endregion

            // 获取技能id和等级
            int skillId = (int)request[(byte)ParameterCode.SkillId];
            int level = (int)request[(byte)ParameterCode.SkillLevel];
            // 使用技能
            DtoDamage[] damages = SkillManager.Instance.UseSkill(skillId, level, from, to, room);

            Dictionary<byte, object> data = request.Parameters;
            if (damages != null)
            {
                // 添加伤害数据
                data.Add((byte)ParameterCode.DtoDamages, JsonMapper.ToJson(damages));
            }
            // 广播使用技能的数据
            room.Brocast(this.OpCode, data);
        }
    }
}

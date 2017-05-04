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
            MobaServer.LogInfo("处理计算伤害的请求");

            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
            // 获取攻击者的id
            int fromId = (int) request[(byte)ParameterCode.AttackId];
            // 获取被攻击者id数组
            int[] toIds = JsonMapper.ToObject<int[]>(request[(byte)ParameterCode.TargetArray] as string);
            // 获取技能id
            int skillId = (int) request[(byte)ParameterCode.SkillId];

            #region 获取攻击者的数据

            DtoMinion fromDto = null;
            // 英雄的id和玩家id一致 所以大于等于0的id是英雄
            if (fromId >= 0)
            {
                fromDto = room.GetDtoHero(fromId);

            }
            // 建筑id范围
            else if (fromId >= ServerConfig.TeamTwoBuildId - 100 && fromId <= ServerConfig.TeamOneBuildId)
            {
                fromDto = room.GetDtoBuild(fromId);
            }
            // 小兵id范围
            else if (fromId <= ServerConfig.MinionId)
            {
                
            }

            #endregion

            #region 获取被攻击者的数据

            DtoMinion[] toDtos = new DtoMinion[toIds.Length];
            for (int i = 0; i < toIds.Length; i++)
            {
                // 英雄的id和玩家id一致 所以大于0的id是英雄
                if (toIds[i] >= 0)
                {
                    toDtos[i] = room.GetDtoHero(toIds[i]);

                }
                // 建筑id范围
                else if (toIds[i] >= ServerConfig.TeamTwoBuildId - 100 && toIds[i] <= ServerConfig.TeamOneBuildId)
                {
                    toDtos[i] = room.GetDtoBuild(toIds[i]);
                }
                // 小兵id范围
                else if (toIds[i] <= ServerConfig.MinionId)
                {
                    
                }
            }
           
            #endregion

            // 使用技能 获取伤害数据
            DtoDamage[] damages = null;
            damages = SkillManager.Instance.RunSkill(skillId, 0, fromDto, toDtos);
            if (damages == null)
                return;

            //// 区别普通攻击和技能
            //if (skillId == ServerConfig.SkillId)
            //{
            //    // 普通攻击
            //    skill = DamageData.GetSkill(skillId);
            //    damages = skill.Damge(0, fromDto, toDtos);

            //    // 死亡检测
            //    foreach (DtoDamage item in damages)
            //    {
            //        if (item.IsDead)
            //        {
                        
            //        }
            //    }
            //}
            //else
            //{
            //    // 技能
            //}

            // 广播伤害数据传输对象
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.DtoDamages, JsonMapper.ToJson(damages));
            room.Brocast(OperationCode.Damage, data);
        }
    }
}

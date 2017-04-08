using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Dto;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// 管理选人的数据
    /// </summary>
    public class SelectData
    {
        /// <summary>
        /// 保存队伍1的数据
        /// </summary>
        public SelectModel[] Team1;
        /// <summary>
        /// 保存队伍2的数据
        /// </summary>
        public SelectModel[] Team2;
        /// <summary>
        /// 玩家的队伍id
        /// </summary>
        public int TeamId;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void InitData(SelectModel[] team1, SelectModel[] team2)
        {
            Team1 = team1;
            Team2 = team2;

            // 判断玩家在哪个队伍
            GetTeamId();
        }

        /// <summary>
        /// 获取玩家的队伍id
        /// </summary>
        /// <returns></returns>
        public void GetTeamId()
        {
            int playerId = GameData.player.Id;
            for (int i = 0; i < Team1.Length; i++)
            {
                if (playerId == Team1[i].PlayerId)
                {
                    TeamId = 1;
                    return;
                }
            }
            for (int i = 0; i < Team2.Length; i++)
            {
                if (playerId == Team2[i].PlayerId)
                {
                    TeamId = 2;
                    return;
                }
            }
        }

        /// <summary>
        /// 新的玩家进入房间时 更新数据
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>返回同组的玩家名</returns>
        public string OnEnterSelect(int playerId)
        {
            foreach (SelectModel model in Team1)
            {
                if (model.PlayerId == playerId)
                {
                    model.IsEnter = true;
                    if (TeamId == 1)
                        return model.PlayerName;
                    return null;
                }
            }
            foreach (SelectModel model in Team2)
            {
                if (model.PlayerId == playerId)
                {
                    model.IsEnter = true;
                    if (TeamId == 2)
                        return model.PlayerName;
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 当玩家选择了英雄
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="heroId"></param>
        public void OnSelected(int playerId, int heroId)
        {
            foreach (SelectModel model in Team1)
            {
                if (model.PlayerId == playerId)
                {
                    model.HeroId = heroId;
                    return;
                }
            }
            foreach (SelectModel model in Team2)
            {
                if (model.PlayerId == playerId)
                {
                    model.HeroId = heroId;
                    return;
                }
            }
        }

        /// <summary>
        /// 当玩家准备完毕
        /// </summary>
        /// <param name="playerId"></param>
        public void OnReady(int playerId)
        {
            foreach (SelectModel model in Team1)
            {
                if (model.PlayerId == playerId)
                {
                    model.IsReady = true;
                    return;
                }
            }
            foreach (SelectModel model in Team2)
            {
                if (model.PlayerId == playerId)
                {
                    model.IsReady = true;
                    return;
                }
            }
        }
    }
}

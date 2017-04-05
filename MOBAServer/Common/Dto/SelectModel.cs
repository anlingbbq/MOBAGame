using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dto
{
    /// <summary>
    /// 选人的数据模型
    /// </summary>
    public class SelectModel
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        public int PlayerId;
        /// <summary>
        /// 选择英雄id
        /// </summary>
        public int HeroId;
        /// <summary>
        /// 玩家id
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 是否进入
        /// </summary>
        public bool IsEnter;
        /// <summary>
        /// 是否准备
        /// </summary>
        public bool IsReady;

        public SelectModel(int playerId, string name)
        {
            this.PlayerId = playerId;
            this.PlayerName = name;
            this.HeroId = -1;
            this.IsEnter = false;
            this.IsReady = false;
        }
    }
}       

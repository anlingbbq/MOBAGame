using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Code
{
    // 区分通信的参数类型
    public enum ParameterCode
    {
        #region 用户

        /// <summary>
        /// 用户名
        /// </summary>
        Username,
        /// <summary>
        /// 密码
        /// </summary>
        Password,

        #endregion

        #region 玩家

        /// <summary>
        /// 玩家数据传输对象
        /// </summary>
        DtoPlayer,
        /// <summary>
        /// 好友数据传输对象
        /// </summary>
        DtoFriend,
        /// <summary>
        /// 玩家姓名
        /// </summary>
        PlayerName,
        /// <summary>
        /// 玩家id
        /// </summary>
        PlayerId,
        /// <summary>
        /// 是否同意添加好友
        /// </summary>
        AcceptAddFriend,
        /// <summary>
        /// 玩家id数组
        /// </summary>
        PlayerIds,

        #endregion

        #region 匹配

        /// <summary>
        /// 第一组的选择数据
        /// </summary>
        TeamOneSelectData,
        /// <summary>
        /// 第二组的选择数据
        /// </summary>
        TeamTwoSelectData,
        /// <summary>
        /// 英雄id
        /// </summary>
        HeroId,
        /// <summary>
        /// 队伍id
        /// </summary>
        TeamId,
        /// <summary>
        /// 聊天内容
        /// </summary>
        TalkContent,

        #endregion

        #region 战斗

        /// <summary>
        /// 英雄数组
        /// </summary>
        HerosArray,
        /// <summary>
        /// 建筑数组
        /// </summary>
        BuildsArray,
        /// <summary>
        /// 物品数组
        /// </summary>
        ItemArray,
        /// <summary>
        /// 英雄传输对象
        /// </summary>
        DtoHero,
        /// <summary>
        /// 三维坐标
        /// </summary>
        DtoVector3,
        /// <summary>
        /// 目标id
        /// </summary>
        TargetId,
        /// <summary>
        /// 目标id数组
        /// </summary>
        TargetArray,
        /// <summary>
        /// 发起者id
        /// </summary>
        FromId,
        /// <summary>
        /// 技能id
        /// </summary>
        SkillId,
        /// <summary>
        /// 伤害数据
        /// </summary>
        DtoDamages,
        /// <summary>
        /// 装备Id
        /// </summary>
        ItemId,
        /// <summary>
        /// 技能等级
        /// </summary>
        SkillLevel,
        /// <summary>
        /// 技能模型数组
        /// </summary>
        SkillArray,

        #endregion
    }
}

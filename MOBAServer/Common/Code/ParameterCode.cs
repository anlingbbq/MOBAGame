using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Code
{
    // 区分通信的参数类型
    public enum ParameterCode
    {
        /// <summary>
        /// 用户名
        /// </summary>
        Username,
        /// <summary>
        /// 密码
        /// </summary>
        Password,
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
        PlayerIds
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.OpCode
{
    // 区分通信的操作类型
    public enum OperationCode
    {
        Defalut,
        /// <summary>
        /// 用户登陆
        /// </summary>
        UserLogin,
        /// <summary>
        /// 用户注册
        /// </summary>
        UserRegister,
        /// <summary>
        /// 创建玩家
        /// </summary>
        PlayerCreate,
        /// <summary>
        /// 获取玩家信息
        /// </summary>
        PlayerGetInfo,
        /// <summary>
        /// 玩家上线
        /// </summary>
        PlayerOnline,
        /// <summary>
        /// 请求添加好友
        /// </summary>
        PlayerAddRequest,
        /// <summary>
        /// 回应添加好友
        /// </summary>
        PlayerAddToClient,
        /// <summary>
        /// 添加好友的结果
        /// </summary>
        PlayerAddResult,
        /// <summary>
        /// 好友登陆状态改变
        /// </summary>
        FriendStateChange,
        /// <summary>
        /// 开始匹配
        /// </summary>
        StartMatch,
        /// <summary>
        /// 停止匹配
        /// </summary>
        StopMatch
    }
}

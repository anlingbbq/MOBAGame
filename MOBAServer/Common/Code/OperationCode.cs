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
        UserLogin,
        UserRegister,
        PlayerCreate,
        PlayerGetInfo,
        PlayerOnline,
        PlayerAddRequest,
        PlayerAddToClient,
        PlayerAddResult,
        FriendOnline
    }
}

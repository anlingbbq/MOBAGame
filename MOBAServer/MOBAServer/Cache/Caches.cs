﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Cache
{
    /// <summary>
    /// 缓存层 减少数据库调用
    /// </summary>
    public class Caches
    {
        public static UserCache User;
        public static PlayerCache Player;

        static Caches()
        {
            User = new UserCache();
            Player = new PlayerCache();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Cache
{
    /// <summary>
    /// 缓存层
    /// TODO 所有的缓存均没有释放
    /// </summary>
    public class Caches
    {
        public static UserCache User;
        public static PlayerCache Player;
        public static MatchCache Match;
        public static SelectCache Select;

        static Caches()
        {
            User = new UserCache();
            Player = new PlayerCache();
            Match = new MatchCache();
            Select = new SelectCache();
        }
    }
}

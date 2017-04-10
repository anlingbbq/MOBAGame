
namespace MOBAServer.Cache
{
    /// <summary>
    /// 缓存层
    /// </summary>
    public class Caches
    {
        // TODO User,Player的数据没有做过释放的处理
        public static UserCache User;
        public static PlayerCache Player;

        public static MatchRoomCache Match;
        public static SelectRoomCache Select;
        public static BattleRoomCache Battle;

        static Caches()
        {
            User = new UserCache();
            Player = new PlayerCache();
            Match = new MatchRoomCache();
            Select = new SelectRoomCache();
            Battle = new BattleRoomCache();
        }
    }
}

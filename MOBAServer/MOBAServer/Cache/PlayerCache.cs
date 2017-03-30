using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.DataBase.Manager;
using MOBAServer.DataBase.Model;
using MOBAServer.Extension;

namespace MOBAServer.Cache
{
    // 缓存玩家(角色)信息
    /*
     * TODO PlayerList内存释放
     * 同样这里的内存没有释放的机会 需要在User释放时一起释放
     */
    public class PlayerCache
    {
        #region 缓存用户玩家数据

        // 保存玩家数据 用户名和玩家列表的映射
        private Dictionary<string, IList<Player>> m_PlayerDict = new Dictionary<string, IList<Player>>();

        // 添加玩家数据
        public void AddPlayer(string username, Player player)
        {
            m_PlayerDict.ExTryGet(username).Add(player);
        }

        // 添加玩家列表
        public void AddPlayerList(string username)
        {
            IList<Player> playerList = new List<Player>();
            UserManager.CachePlayerList(username, playerList);
            m_PlayerDict.Add(username, playerList);

            PrintPlayerList(playerList);
        }

        // 用户是否创建有玩家
        public bool HasPlayer(string username)
        {
            return m_PlayerDict.ExTryGet(username).Count > 0;
        }

        // 获得用户的玩家列表
        public IList<Player> GetPlayerList(string username)
        {
            return m_PlayerDict.ExTryGet(username);
        }

        // 输出所有的玩家信息
        public void PrintPlayerList(IList<Player> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int id = list[i].Identification;
                string Name = list[i].Name;
                int Power = list[i].Power;
                int RunCount = list[i].RunCount;
                string info = ">>>>>>>> id : " + id + " name : " + Name + " Power : " + Power + " RunCount : " + RunCount;
                MobaServer.LogInfo(info);
            }
        }

        #endregion

        #region 缓存上线的玩家数据

        // 保存上线的玩家 玩家id和客户端连接的映射
        private Dictionary<int, MobaPeer> m_OnlineDict = new Dictionary<int, MobaPeer>();

        public bool IsOnline(int id)
        {
            return m_OnlineDict.ContainsKey(id);
        }

        public void Online(int id, MobaPeer peer)
        {
            if (m_OnlineDict.ContainsKey(id))
                return;

            m_OnlineDict.Add(id, peer);
        }

        public void Offline(string username)
        {
            int playerId = UserManager.GetPlayer(username).Identification;

            if (!m_OnlineDict.ContainsKey(playerId))
                return;

            m_OnlineDict.Remove(playerId);
        }

        public MobaPeer GetPeer(int id)
        {
            return m_OnlineDict.ExTryGet(id);
        }

        #endregion
    }
}

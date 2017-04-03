using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.DTO;
using Common.OpCode;
using LitJson;
using MOBAServer.DataBase.Manager;
using MOBAServer.DataBase.Model;
using MOBAServer.Extension;
using Photon.SocketServer;

namespace MOBAServer.Cache
{
    /// <summary>
    /// 缓存玩家(角色)信息
    /// 这里只缓存登陆过的用户的信息
    /// </summary>
    public class PlayerCache
    {
        #region 缓存用户的玩家列表

        // 保存用户名和玩家列表的映射
        private Dictionary<string, IList<Player>> m_PlayerListDict = new Dictionary<string, IList<Player>>();

        /// <summary>
        /// 根据用户名添加到用户的玩家列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="player"></param>
        public void AddPlayer(string username, Player player)
        {
            m_PlayerListDict.ExTryGet(username).Add(player);
        }

        /// <summary>
        /// 添加玩家列表 
        /// </summary>
        /// <param name="username"></param>
        public void AddPlayerList(string username)
        {
            IList<Player> playerList = new List<Player>();
            UserManager.CachePlayerList(username, playerList);
            m_PlayerListDict.Add(username, playerList);

            //PrintPlayerList(playerList);
        }

        /// <summary>
        /// 用户是否创建有玩家 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool HasPlayer(string username)
        {
            return m_PlayerListDict.ExTryGet(username).Count > 0;
        }

        /// <summary>
        /// 获得用户的玩家列表 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<Player> GetPlayerList(string username)
        {
            return m_PlayerListDict.ExTryGet(username);
        }

        /// <summary>
        /// 输出用户的所有玩家信息 
        /// </summary>
        /// <param name="list"></param>
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

        #region 缓存玩家数据

        // 保存玩家id和玩家数据的映射
        private Dictionary<int, Player> m_PlayerDict = new Dictionary<int, Player>();

        /// <summary>
        /// 缓存玩家数据
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            if (!m_PlayerDict.ContainsKey(player.Identification))
                m_PlayerDict.Add(player.Identification, player);
        }

        /// <summary>
        /// 是否存在玩家数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExistPlayer(int id)
        {
            return m_PlayerDict.ContainsKey(id);
        }

        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer(int id)
        {
            return m_PlayerDict.ExTryGet(id);
        }

        /// <summary>
        /// 根据名称获取玩家数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Player GetPlayerByName(string name)
        {
            foreach (Player player in m_PlayerDict.Values)
            {
                if (player.Name == name)
                    return player;
            }
            return null;
        }

        #endregion

        #region 缓存上线的玩家数据

        // 保存上线的玩家 玩家id和客户端连接的映射
        private Dictionary<int, MobaPeer> m_OnlineDict = new Dictionary<int, MobaPeer>();

        /// <summary>
        /// 玩家是否上线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsOnline(int id)
        {
            return m_OnlineDict.ContainsKey(id);
        }

        /// <summary>
        /// 上线添加缓存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="peer"></param>
        public void Online(int id, MobaPeer peer)
        {
            if (m_OnlineDict.ContainsKey(id))
                return;

            m_OnlineDict.Add(id, peer);
        }

        /// <summary>
        /// 下线移除缓存
        /// </summary>
        /// <param name="username"></param>
        public void Offline(MobaPeer peer)
        {
            Player player = UserManager.GetPlayer(peer.Username);

            if (!m_OnlineDict.ContainsKey(player.Identification))
                return;

            // 通知所有好友下线
            if (!string.IsNullOrEmpty(player.FriendIdList))
            {
                OperationResponse response = new OperationResponse((byte) OperationCode.FriendStateChange);
                string[] friendList = player.FriendIdList.Split(',');
                foreach (string friendId in friendList)
                {
                    int id = int.Parse(friendId);
                    if (IsOnline(id))
                    {
                        response.Parameters = new Dictionary<byte, object>();
                        MobaPeer tempPeer = GetPeer(id);
                        
                        DtoFriend dto = new DtoFriend(id, player.Name, false);
                        response[(byte)ParameterCode.DtoFriend] = JsonMapper.ToJson(dto);
                        tempPeer.SendOperationResponse(response, new SendParameters());
                    }
                }
            }

            m_OnlineDict.Remove(player.Identification);
        }

        /// <summary>
        /// 通过玩家id获取上线的客户端连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MobaPeer GetPeer(int id)
        {
            return m_OnlineDict.ExTryGet(id);
        }

        #endregion
    }
}

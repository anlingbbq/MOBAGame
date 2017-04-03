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
    /// <summary>
    /// 缓存用户信息 
    /// 这里只缓存登陆过的用户的信息
    /// </summary>
    public class UserCache
    {
        #region 缓存客户端连接

        // 保存登陆的客户端连接
        private Dictionary<string, MobaPeer> m_PeerDict = new Dictionary<string, MobaPeer>();

        /// <summary>
        /// 玩家是否在线 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsOnline(string username)
        {
            return m_PeerDict.ContainsKey(username);
        }

        /// <summary>
        /// 在线添加缓存 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="peer"></param>
        public void Online(string username, MobaPeer peer)
        {
            if (m_PeerDict.ContainsKey(username))
                return;

            peer.Username = username;
            m_PeerDict.Add(username, peer);

            // 添加玩家数据
            Caches.User.AddUser(username);
        }

        /// <summary>
        /// 离线移除上线缓存 
        /// </summary>
        /// <param name="username"></param>
        public void Offline(string username)
        {
            if (!m_PeerDict.ContainsKey(username))
                return;

            m_PeerDict.Remove(username);
        }

        /// <summary>
        /// 验证连接对象是否已经登陆 
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public bool VerifyPeer(MobaPeer peer)
        {
            return m_PeerDict.ContainsKey(peer.Username);
        }

        #endregion

        #region 缓存用户数据

        // 保存用户数据库对象
        private Dictionary<string, User> m_UserDict = new Dictionary<string, User>();

        /// <summary>
        /// 是否存在用户数据 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsExistUser(string username)
        {
            return m_UserDict.ContainsKey(username);
        }

        /// <summary>
        /// 获取用户数据 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            return m_UserDict[username];
        }

        /// <summary>
        /// 添加用户数据 
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            m_UserDict.Add(user.Name, user);

            // 在添加用户数据的时候 添加对应的玩家列表
            Caches.Player.AddPlayerList(user.Name);
        }

        /// <summary>
        /// 添加用户数据 
        /// </summary>
        /// <param name="username"></param>
        public void AddUser(string username)
        {
            if (!m_UserDict.ContainsKey(username))
            {
                User user = UserManager.GetByUsername(username);
                m_UserDict.Add(user.Name, user);

                // 在添加用户数据的时候 添加对应的玩家列表
                Caches.Player.AddPlayerList(user.Name);
            }
        }

        /// <summary>
        /// 核实用户名和密码 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyUser(string username, string password)
        {
            return m_UserDict.ExTryGet(username).Password == password;
        }

        #endregion
    }
}

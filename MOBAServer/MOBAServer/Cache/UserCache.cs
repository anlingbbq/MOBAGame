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
    // 缓存用户信息
    public class UserCache
    {
        #region 缓存客户端连接

        // 保存登陆的客户端连接
        private Dictionary<string, MobaPeer> m_PeerDict = new Dictionary<string, MobaPeer>();

        // 玩家是否在线
        public bool IsOnline(string username)
        {
            return m_PeerDict.ContainsKey(username);
        }

        // 在线添加缓存
        public void Online(string username, MobaPeer peer)
        {
            if (m_PeerDict.ContainsKey(username))
                return;

            peer.Username = username;
            m_PeerDict.Add(username, peer);

            // 添加玩家数据
            Caches.User.AddUser(username);
        }

        // 离线移除上线缓存
        public void Offline(string username)
        {
            if (!m_PeerDict.ContainsKey(username))
                return;

            m_PeerDict.Remove(username);
        }

        // 验证连接对象是否已经登陆
        public bool VerifyPeer(MobaPeer peer)
        {
            return m_PeerDict.ContainsKey(peer.Username);
        }

        #endregion


        /*
         * TODO User内存释放
         * 这里的用户缓存并没有自动释放的机会 内存会持续增加
         */
        #region 缓存用户数据

        // 保存用户数据库对象
        private Dictionary<string, User> m_UserDict = new Dictionary<string, User>();

        // 是否存在用户数据
        public bool IsExistUser(string username)
        {
            return m_UserDict.ContainsKey(username);
        }

        // 获取用户数据
        public User GetUser(string username)
        {
            return m_UserDict[username];
        }

        // 添加用户数据
        public void AddUser(User user)
        {
            m_UserDict.Add(user.Name, user);

            // 在添加用户数据的时候 添加对应的玩家列表
            Caches.Player.AddPlayerList(user.Name);
        }

        // 添加用户数据
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

        // 核实用户名和密码
        public bool VerifyUser(string username, string password)
        {
            return m_UserDict.ExTryGet(username).Password == password;
        }

        #endregion
    }
}

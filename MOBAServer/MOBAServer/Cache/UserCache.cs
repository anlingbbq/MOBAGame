using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.DataBase.Model;
using MOBAServer.Extension;

namespace MOBAServer.Cache
{
    // 缓存用户信息
    public class UserCache
    {
        // 保存登陆的客户端
        private Dictionary<string, MobaPeer> m_PeerDict = new Dictionary<string, MobaPeer>();

        // 玩家是否在线
        public bool IsOnLine(string username)
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
        }

        // 离线移除缓存
        public void OffLine(string username)
        {
            if (!m_PeerDict.ContainsKey(username))
            {
                MobaServer.LogWarn("账号非法登录");
                return;
            }
            m_PeerDict.Remove(username);
        }

        // 验证连接对象是否已经登陆
        public bool VerifyPeer(MobaPeer peer)
        {
            return m_PeerDict.ContainsKey(peer.Username);
        }
    }
}

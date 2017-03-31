using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.Cache;
using MOBAServer.DataBase.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MOBAServer.DataBase.Manager
{
    /// <summary>
    /// 封装对玩家数据的管理
    /// 先进行内存操作 然后对数据库操作
    /// 
    /// 这里我只缓存了登陆过的用户信息
    /// 也可以在每次执行sql查询的之后缓存该信息
    /// 从而缓存所有使用过的信息
    /// </summary>
    public class PlayerManager : BaseManager
    {
        /// <summary>
        /// 通过玩家名查找 
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public static Player VerifyPlayer(string playerName)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                Player player = session.CreateCriteria(typeof(Player))
                    .Add(Restrictions
                    .Eq("Name", playerName))
                    .UniqueResult<Player>();

                return player;
            }
        }

        /// <summary>
        /// 添加玩家数据 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="player"></param>
        public static void Add(string username, Player player)
        {
            // 这里调用父类的Add就可以了
            // 没有必要再对User对象的PlayerList进行操作 更新User表
            // 在数据库中是以外键关联的方式 User没有更新的东西
            // 而在程序中获取PlayerList会再次执行关联查询
            // 没有维护User的必要
            BaseManager.Add(player);

            // 缓存到对应的玩家列表
            Caches.Player.AddPlayer(username, player);
            // 缓存玩家数据
            Caches.Player.AddPlayer(player);
        }

        public static Player GetById(int id)
        {
            if (Caches.Player.IsExistPlayer(id))
                return Caches.Player.GetPlayer(id);

            return BaseManager.GetById<Player>(id);
        }

        /// <summary>
        /// 通过用户名获取玩家数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Player GetByName(string name)
        {
            Player player = Caches.Player.GetPlayerByName(name);
            if (player != null) return player;

            using (ISession session = NhibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(Player))
                    .Add(Restrictions.Eq("Name", name))
                    .UniqueResult<Player>();
            }
        }
    }
}

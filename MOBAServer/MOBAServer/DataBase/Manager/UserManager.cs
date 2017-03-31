using System;
using System.Collections.Generic;
using Common.DTO;
using MOBAServer.Cache;
using MOBAServer.DataBase.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MOBAServer.DataBase.Manager
{
    /// <summary>
    /// 封装对用户数据的管理
    /// 先进行内存操作 然后对数据库操作
    /// 
    /// 这里我只缓存了登陆过的用户信息
    /// 也可以在每次执行sql查询的之后缓存该信息
    /// 从而缓存所有使用过的信息
    /// </summary>
    public class UserManager : BaseManager
    {
        /// <summary>
        /// 增加用户 
        /// </summary>
        /// <param name="user"></param>
        public static void Add(User user)
        {
            BaseManager.Add(user);

            Caches.User.AddUser(user);
        }

        /// <summary>
        /// 通过用户名查找
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User GetByUsername(string username)
        {
            if (Caches.User.IsExistUser(username))
                return Caches.User.GetUser(username);

            using (ISession session = NhibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>();

                return user;
            }
        }

        /// <summary>
        /// 核实用户名和密码 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool VerifyUser(string username, string password)
        {
            if (Caches.User.IsExistUser(username))
                return Caches.User.VerifyUser(username, password);

            using (ISession session = NhibernateHelper.OpenSession())
            {
                User user = session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();

                if (user == null)
                    return false;
                return true;
            }
        }


        /// <summary>
        /// 判断用户是否存在角色
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool HasPlayer(string username)
        {
            if (Caches.User.IsExistUser(username))
                return Caches.Player.HasPlayer(username);

            using (ISession session = NhibernateHelper.OpenSession())
            {
                IList<Player> playerList = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>().PlayerList;

                if (playerList.Count > 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 获取用户的第一个玩家数据 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Player GetPlayer(string username)
        {
            if (Caches.User.IsExistUser(username))
                return Caches.Player.GetPlayerList(username)[0];

            using (ISession session = NhibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>().PlayerList[0];
            }
        }


        /// <summary>
        /// 缓存用户的玩家列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="list"></param>
        public static void CachePlayerList(string username, IList<Player> list)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IList<Player> playerList = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>().PlayerList;

                // 复制用户下的玩家列表
                for (int i = 0; i < playerList.Count; i++)
                {
                    list.Add(playerList[i]);
                    // 缓存玩家数据
                    Caches.Player.AddPlayer(playerList[i]);
                }
            }
        }

        /// <summary>
        /// 从缓存中获取用户的玩家列表 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IList<Player> GetPlayerList(string username)
        {
            return Caches.Player.GetPlayerList(username);
        }
    }
}

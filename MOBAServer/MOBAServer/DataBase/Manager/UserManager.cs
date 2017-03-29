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
    /// </summary>
    public class UserManager : BaseManager
    {
        // 增加用户
        public static void Add(User user)
        {
            BaseManager.Add(user);

            Caches.User.AddUser(user);
        }

         // 通过用户名查找
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

        // 核实用户名和密码
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

        // 判断用户是否存在角色
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

        // 获取用户的第一个玩家数据
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

        // 缓存用户的玩家列表
        public static void CachePlayerList(string username, IList<Player> list)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IList<Player> playerList = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>().PlayerList;

                // 深度复制
                for (int i = 0; i < playerList.Count; i++)
                {
                    list.Add(playerList[i]);
                }
            }
        }

        // 从缓存中获取用户的玩家列表
        public static IList<Player> GetPlayerList(string username)
        {
            return Caches.Player.GetPlayerList(username);
        }
    }
}

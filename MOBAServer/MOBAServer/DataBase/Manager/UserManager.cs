using System;
using System.Collections.Generic;
using Common.DTO;
using MOBAServer.DataBase.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MOBAServer.DataBase.Manager
{
    /// <summary>
    /// 封装对数据库中User表的操作
    /// </summary>
    public class UserManager : BaseManager
    {
        // 通过用户名查找
        public static User GetByUsername(string username)
        {
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

        // 添加用户对应的玩家数据
        public static void AddPlayer(Player player)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(player);
                    // 这里这样就可以了
                    // 没有必要再对User对象的PlayerList进行操作 然后更新User表
                    // 在数据库中是以外键关联的方式 User没有更新的东西
                    // 而在程序中获取PlayerList会再次执行关联查询
                    // 所以 在这一点上没有维护User的必要

                    // 这个函数只是为了提醒这一点

                    transaction.Commit();
                }
            }
        }

        // 获取所有用户的玩家列表
        public static IList<Player> GetPlayerList(string username)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IList<Player> playerList = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>().PlayerList;

                return playerList;
            }
        }
    }
}

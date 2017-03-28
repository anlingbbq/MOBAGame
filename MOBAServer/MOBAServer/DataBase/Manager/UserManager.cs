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
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>();

                if (user.PlayerList.Count > 0)
                    return true;
                return false;
            }
        }

        // 获取所有用户的玩家列表
        public static IList<Player> GetPlayerList(string username)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Name", username))
                    .UniqueResult<User>();

                return user.PlayerList;
            }
            //using (ISession session = NhibernateHelper.OpenSession())
            //{
            //    return session.CreateCriteria(typeof(User))
            //        .CreateCriteria("PlayerList")
            //        .SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer())
            //        .List<Player>();
            //}
        }
    }
}

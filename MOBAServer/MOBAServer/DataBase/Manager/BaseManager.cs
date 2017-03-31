using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace MOBAServer.DataBase.Manager
{
    /// <summary>
    /// 基础的数据库对象管理
    /// 提供增，删，改，查等操作
    /// </summary>
    public class BaseManager
    {
        /// <summary>
        /// 增加对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Add<T>(T t)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(t);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// 移除对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Remove<T>(T t)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(t);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// 修改对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Update<T>(T t)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(t);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// 通过id查找对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetById<T>(int id)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    T t = session.Get<T>(id);
                    transaction.Commit();
                    return t;
                }
            }
        }

        /// <summary>
        /// 获取所有该类型的对象列表 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICollection<T> GetAll<T>()
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IList<T> list = session.CreateCriteria(typeof(T)).List<T>();
                return list;
            }
        }
    }
}

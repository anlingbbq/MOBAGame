using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.DataBase.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MOBAServer.DataBase.Manager
{
    /// <summary>
    /// 封装对数据库中User表的操作
    /// </summary>
    public class UserManager : IUserManager
    {
        private static UserManager _Instanc;

        public static UserManager Instance
        {
            get
            {
                if (_Instanc == null)
                {
                    _Instanc = new UserManager();
                }
                return _Instanc;
            }
        }

        public void Add(User user)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public void Update(User user)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }

        public void Remove(User user)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }

        public User GetById(int id)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    User user = session.Get<User>(id);
                    transaction.Commit();
                    return user;
                }
            }
        }

        public User GetByUsername(string username)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", username))
                    .UniqueResult<User>();
            }
        }

        public ICollection<User> GetAllUser()
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(User)).List<User>();
            }
        }

        public bool VerifyUser(string username)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                User user = session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", username))
                    .UniqueResult<User>();

                if (user == null)
                    return false;
                else return true;
            }
        }

        public bool VerifyUser(string username, string password)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                User user = session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", username))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();

                if (user == null)
                    return false;
                else return true;
            }
        }
    }
}

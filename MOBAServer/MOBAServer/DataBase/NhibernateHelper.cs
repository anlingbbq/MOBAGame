using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace MOBAServer.DataBase
{
    class NhibernateHelper
    {
        private static ISessionFactory m_SessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (m_SessionFactory == null)
                {
                    var configuration = new Configuration();
                    // 解析hibernate.cfg.xml
                    configuration.Configure();
                    // 解析程序集中的映射文件
                    configuration.AddAssembly("MobaServer");

                    m_SessionFactory = configuration.BuildSessionFactory();
                }
                return m_SessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}

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
        private static ISessionFactory _SessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_SessionFactory == null)
                {
                    var configuration = new Configuration();
                    // 解析hibernate.cfg.xml
                    configuration.Configure();
                    // 解析程序集中的映射文件
                    configuration.AddAssembly("MobaServer");

                    _SessionFactory = configuration.BuildSessionFactory();
                }
                return _SessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}

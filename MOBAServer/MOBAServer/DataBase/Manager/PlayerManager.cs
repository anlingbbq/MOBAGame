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
    public class PlayerManager : BaseManager
    {
        // 通过玩家名查找
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
    }
}

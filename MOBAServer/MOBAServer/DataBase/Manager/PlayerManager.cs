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

        // 添加玩家数据
        public static void Add(string username, Player player)
        {
            // 这里调用父类的Add就可以了
            // 没有必要再对User对象的PlayerList进行操作 更新User表
            // 在数据库中是以外键关联的方式 User没有更新的东西
            // 而在程序中获取PlayerList会再次执行关联查询
            // 没有维护User的必要
            BaseManager.Add(player);

            // 添加到对应的玩家列表
            Caches.Player.AddPlayer(username, player);
        }
    }
}

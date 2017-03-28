using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Cache
{
    /// <summary>
    /// 基础的对象缓存类 保存数据库对象 减少数据库调用
    /// 提供增，删，改，查等操作
    /// </summary>
    public class BaseCache<TK, TV>
    {
        // 保存数据库对象
        private Dictionary<TK, TV> m_DataDict = new Dictionary<TK, TV>();

        // 增加对象
        public bool Add(TK k, TV v)
        {
            if (m_DataDict.ContainsKey(k))
                return false;
            else
            {
                m_DataDict.Add(k, v);
                return true;
            }
        }

        // 删除对象
        public void Rmove(TK k, TV v)
        {
            if (m_DataDict.ContainsKey(k))
            {
                m_DataDict.Remove(k);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DTO
{
    /// <summary>
    /// 好友传输对象
    /// </summary>
    public class DtoFriend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }

        public DtoFriend()
        {
            
        }

        public DtoFriend(int id, string name, bool isOnline)
        {
            this.Id = id;
            this.Name = name;
            this.IsOnline = isOnline;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DTO
{
    /// <summary>
    /// 玩家传输对象
    /// </summary>
    public class DtoPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Lv { get; set; }
        public int Exp { get; set; }
        public int Power { get; set; }
        public int RunCount { get; set; }
        public int WinCount { get; set; }
        public int LostCount { get; set; }
        public List<int> HeroIds { get; set; }
        public List<DtoFriend> Friends { get; set; }

        public  DtoPlayer()
        {
            HeroIds = new List<int>();
            Friends = new List<DtoFriend>();
        }
    }
}

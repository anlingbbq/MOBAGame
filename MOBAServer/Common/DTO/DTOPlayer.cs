using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DTO
{
    public class DTOPlayer
    {
        public string Name { get; set; }
        public int Lv { get; set; }
        public int Exp { get; set; }
        public int Power { get; set; }
        public int RunCount { get; set; }
        public int WinCount { get; set; }
        public int LostCount { get; set; }
        public string HeroIdList { get; set; }
        public string FriendIdList { get; set; }
    }
}

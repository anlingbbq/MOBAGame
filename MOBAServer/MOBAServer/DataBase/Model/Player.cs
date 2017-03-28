using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace MOBAServer.DataBase.Model
{
    public class Player
    {
        public Player()
        {
            
        }

        public Player(string name, User user)
        {
            Name = name;
            User = user;

            Lv = 1;
            Exp = 0;
            Power = 2000;
            RunCount = 0;
            WinCount = 0;
            LostCount = 0;
            HeroIdList = "0, 1";
            FriendIdList = "";
        }

        public virtual int Identification { get; set; }
        public virtual string Name { get; set; }
        public virtual int Lv { get; set; }
        public virtual int Exp { get; set; }
        public virtual int Power { get; set; }
        public virtual int RunCount { get; set; }
        public virtual int WinCount { get; set; }
        public virtual int LostCount { get; set; }
        public virtual string HeroIdList { get; set; }
        public virtual string FriendIdList { get; set; }
        public virtual User User { get; set; }

        // 获得数据传输对象
        public virtual DTOPlayer ConvertToDot()
        {
            return new DTOPlayer()
            {
                Name = this.Name,
                Lv = this.Lv,
                Exp = this.Exp,
                Power = this.Power,
                RunCount = this.RunCount,
                WinCount = this.WinCount,
                LostCount = this.LostCount,
                HeroIdList = this.HeroIdList,
                FriendIdList = this.FriendIdList
            };
        }
    }
}

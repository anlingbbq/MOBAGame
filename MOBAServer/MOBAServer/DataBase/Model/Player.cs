using Common.Config;
using Common.DTO;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;

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
            HeroIdList = HeroData.TypeId_Warrior + "," + HeroData.TypeId_Archer;
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
        public virtual DtoPlayer ConvertToDot()
        {
            DtoPlayer dtoPlayer = new DtoPlayer()
            {
                Id = this.Identification,
                Name = this.Name,
                Lv = this.Lv,
                Exp = this.Exp,
                Power = this.Power,
                RunCount = this.RunCount,
                WinCount = this.WinCount,
                LostCount = this.LostCount,
            };

            // 复制英雄列表
            string[] heros = this.HeroIdList.Split(',');
            foreach (string hero in heros)
            {
                dtoPlayer.HeroIds.Add(int.Parse(hero));
            }

            // 复制好友列表
            string[] friends = this.FriendIdList.Split(',');
            foreach (string friend in friends)
            {
                if (string.IsNullOrEmpty(friend))
                    continue;

                int id = int.Parse(friend);
                string name = PlayerManager.GetById(id).Name;
                bool isOnline = Caches.Player.IsOnline(id);
                dtoPlayer.Friends.Add(new DtoFriend(id, name, isOnline));
            }
            return dtoPlayer;
        }
    }
}

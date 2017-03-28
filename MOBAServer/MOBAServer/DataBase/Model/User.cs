using System;
using System.Collections;
using System.Collections.Generic;

namespace MOBAServer.DataBase.Model
{
    public class User
    {
        public User()
        {
            
        }

        public User(string username, string password)
        {
            Name = username;
            Password = password;

            RegisterDate = DateTime.Now;
        }

        public virtual int Identification { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime RegisterDate { get; set; }

        public virtual IList<Player> PlayerList { get; set; }
    }
}

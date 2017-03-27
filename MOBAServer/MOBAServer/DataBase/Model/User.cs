using System;

namespace MOBAServer.DataBase.Model
{
    public class User
    {
        public User() { }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime RegisterDate { get; set; }
    }
}

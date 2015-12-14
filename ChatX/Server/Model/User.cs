using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class User
    {

        public User(string userName, string ip)
        {
            UserName = userName;
            IP = ip;
        }

        public User(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }

        public string IP { get; set; }

        public override bool Equals(object obj)
        {
            User user = (User)obj;

            return UserName.Equals(user.UserName);
        }
    }
}

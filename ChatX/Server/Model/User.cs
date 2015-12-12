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

        public string UserName { get; set; }

        public string IP { get; set; }
    }
}

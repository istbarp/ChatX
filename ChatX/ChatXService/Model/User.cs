using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatXService.Model
{
    public class User
    {

        public User()
        {

        }

        public User(string userName, string password)
        {
            Password = password;
            UserName = userName;
        }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

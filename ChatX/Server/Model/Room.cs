using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Room
    {
        public Room(string roomName, List<User> users)
        {
            RoomName = roomName;
            Users = users;
        }
        public Room(string roomName)
        {
            RoomName = roomName;
        }

        public string RoomName { get; set; }

        public List<User> Users { get; set; }

        public override bool Equals(object obj)
        {
            Room room = (Room)obj;

            return RoomName.Equals(room.RoomName);
        }
    }
}

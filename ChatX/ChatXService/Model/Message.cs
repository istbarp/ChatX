using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatXService.Model
{
    public class Message
    {
        public Message()
        {

        }

        public Message(string text, User sender, int roomID)
        {
            Text = text;
            Sender = sender;
            RoomID = roomID;
        }

        public string Text { get; set; }

        public User Sender { get; set; }

        public int RoomID { get; set; }
    }
}

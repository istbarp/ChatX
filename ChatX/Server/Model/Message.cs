using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Message
    {
        private string text;
        private User sender;
        private int roomID;

        public Message()
        {

        }

        public Message(string text, User sender, int roomID)
        {
            this.text = text;
            this.sender = sender;
            this.roomID = roomID;
        }

        public string Text 
        {
            get { return text; }
            set { text = value; }
        }

        public User Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }
    }
}

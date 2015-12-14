using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.ChatSocket
{
    public class ChatConnection : IChatConnection
    {
        private Socket handler;

        public ChatConnection(Socket handler)
        {
            this.handler = handler;
            MsgEncoder = Encoding.UTF8;
        }

        /// <summary>
        /// Defaults to Encoding.UTF8
        /// </summary>
        public Encoding MsgEncoder {get;set;}

        public void SendMessage(string message)
        {
            byte[] buffer = new byte[1024];

            buffer = MsgEncoder.GetBytes(message);

            handler.Send(buffer);

            MessageSendListeners(message);
        }


        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public event OnMesageSend MessageSendListeners;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatX.Interfaces
{
    interface IClient
    {
        /// <summary>
        /// Send a message to a room (remember to encrypt first)
        /// </summary>
        /// <param name="msg">Message to send to room</param>
        void Send(IMessage msg);

        /// <summary>
        /// Used by some sort of connection to send YOU a message!
        /// </summary>
        /// <param name="encMsg"></param>
        void Recieve(byte[] encMsg);
    }
}

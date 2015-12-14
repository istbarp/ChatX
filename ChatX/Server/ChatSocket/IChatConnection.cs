using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ChatSocket
{
    public delegate void OnMesageSend(string message);

    public interface IChatConnection
    {
        void SendMessage(string message);
        void Disconnect();
        event OnMesageSend MessageSendListeners;
    }
}

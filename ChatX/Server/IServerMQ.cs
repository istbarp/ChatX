using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate void OnCommandRecieveDelegate(string command);

    public interface IServerMQ
    {
        void SendToResponseQueue(string response);
        event OnCommandRecieveDelegate OnCommandReceive;
        void StartListening();
        void StopListening();
    }
}

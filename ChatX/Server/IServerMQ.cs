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
        void RecieveFromCommandQueue();
        event OnCommandRecieveDelegate OnCommandReceive;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatXService
{
    public delegate void CommandDelegate(string command);

    public interface IMQDriver
    {
        void CloseConnections();
        void SendCommand(string command);
        event CommandDelegate OnResponseRecieved;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatXService
{
    interface IServerDestributer
    {
        string RequestServer();
        string[] GetAllServers();
        IMQDriver GetMQDriver(string server);
        int GetServerCount();
    }
}

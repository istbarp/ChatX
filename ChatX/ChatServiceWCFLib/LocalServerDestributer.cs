using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using RabbitMQ.Client;

namespace ChatXService
{
    public class LocalServerDestributer : IServerDestributer
    {
        private readonly int SERVER_PORT = 9966;
        //private readonly string MQ_HOSTNAME = "";
        private readonly string MQ_USERNAME = "test";
        private readonly string MQ_PASSWORD = "test";
        private readonly string MQ_VIRTUAL_HOST = "testhost";


        public IMQDriver GetMQDriver(string server)
        {
            return new RabbitMQDriver(server, MQ_USERNAME, MQ_PASSWORD, MQ_VIRTUAL_HOST);
        }

        public string RequestServer()
        {
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());

            string retVal = String.Format("{0}:{1}", entry.AddressList[0], SERVER_PORT);
            return retVal;
        }


        public int GetServerCount()
        {
            //TODO: make heartbeat to confirm server connections
            return 1;
        }


        public string[] GetAllServers()
        {
            return new string[] { RequestServer() };
        }
    }
}
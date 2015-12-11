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
        private readonly int PORT = 9966;
        //private readonly string MQ_HOSTNAME = "";
        private readonly string MQ_USERNAME = "";
        private readonly string MQ_PASSWORD = "";
        private readonly string MQ_VIRTUAL_HOST = "";


        public IMQDriver GetMQDriver(string server)
        {
            return new RabbitMQDriver(server, MQ_USERNAME, MQ_PASSWORD, MQ_VIRTUAL_HOST);
        }

        public bool CreateUser(string username)
        {
            //TODO: check to see if username is already taken
            return true;
        }

        public string RequestServer()
        {
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());

            string retVal = String.Format("{0}:{1}", entry.AddressList[0], PORT);
            return retVal;
        }


        public int GetServerCount()
        {
            //TODO: make heartbeat to confirm server connections
            return 1;
        }
    }
}
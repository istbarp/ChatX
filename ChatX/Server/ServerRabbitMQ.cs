using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Server
{
    class ServerRabbitMQ : IServerMQ
    {
        public ServerRabbitMQ(string hostName, string responseQueueName)
        {
            HostName = hostName;
            ResponseQueueName = responseQueueName;

            OnCommandReceive += (msg) => { };
            
        }

        private readonly Encoding Q_MSG_ENC = Encoding.UTF8;
        public string HostName { get; set; }
        public string ResponseQueueName { get; set; }

        public void SendToResponseQueue(string response)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = HostName };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: ResponseQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    byte[] body = Q_MSG_ENC.GetBytes(response);

                    //For debugging
                    //Console.WriteLine("Response Message:\"{0}\" has been sent!", response);

                    channel.BasicPublish(exchange: "",
                        routingKey: ResponseQueueName,
                        basicProperties: null,
                        body: body);

                }
            }
        }

        public void RecieveFromCommandQueue()
        {
            throw new NotImplementedException();
        }

        public event OnCommandRecieveDelegate OnCommandReceive;
 
    }
}

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
        public ServerRabbitMQ(string localHostIP, string responseQueueName, string commandQueueName)
        {
            LocalHostIP = localHostIP;
            ResponseQueueName = responseQueueName;
            CommandQueueName = commandQueueName;

            OnCommandReceive += (msg) => { };

        }

        private readonly Encoding Q_MSG_ENC = Encoding.UTF8;
        public string LocalHostIP { get; set; }
        public string ResponseQueueName { get; private set; }
        public string CommandQueueName { get; private set; }

        public event OnCommandRecieveDelegate OnCommandReceive;

        public void SendToResponseQueue(string response)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = LocalHostIP };
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

        public void StartListening()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = LocalHostIP };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: CommandQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                        {
                            byte[] body = ea.Body;
                            string message = Q_MSG_ENC.GetString(body);
                            OnCommandReceive(message);
                        };
                    channel.BasicConsume(queue: CommandQueueName,
                                         noAck: true,
                                         consumer: consumer);
                }
            }
        }
    }
}

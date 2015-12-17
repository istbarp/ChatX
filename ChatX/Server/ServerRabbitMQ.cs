using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;


namespace Server
{
    class ServerRabbitMQ : IServerMQ
    {
        private readonly string RES_Q_NAME = "Reponse Queue";
        private readonly string CMD_Q_NAME = "Command Queue";

        public ServerRabbitMQ(string localHostIP, string remoteHostIP)
        {
            LocalHostIP = localHostIP;
            RemoteHostIP = remoteHostIP;
            ResponseQueueName = RES_Q_NAME;
            CommandQueueName = CMD_Q_NAME;

            OnCommandReceive += (msg) => { };

            IsListening = false;

        }

        private readonly Encoding Q_MSG_ENC = Encoding.UTF8;
        public string LocalHostIP { get; private set; }
        public string RemoteHostIP { get; private set; }
        public string ResponseQueueName { get; private set; }
        public string CommandQueueName { get; private set; }
        public bool IsListening { get; private set; }

        public event OnCommandRecieveDelegate OnCommandReceive;

        public void SendToResponseQueue(string response)
        {
            //TODO: don't initialize the factory every time a response is send
            ConnectionFactory factory = new ConnectionFactory() { HostName =  RemoteHostIP};
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

        public void StopListening()
        {
            IsListening = false;
        }

        public void StartListening()
        {
            IsListening = true;
            Thread listenThread = new Thread(() =>
            {
                ConnectionFactory factory = new ConnectionFactory() { HostName = LocalHostIP };
                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: CommandQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                            {
                                byte[] body = ea.Body;
                                string message = Q_MSG_ENC.GetString(body);
                                Console.WriteLine(message);
                                OnCommandReceive(message);
                            };
                        channel.BasicConsume(queue: CommandQueueName,
                                             noAck: false,
                                             consumer: consumer);

                        //TODO: find better way to keep connection open
                        while (IsListening)
                        {
                            Thread.Sleep(500);
                        }
                    }
                }
            });

            listenThread.Start();
        }
    }
}

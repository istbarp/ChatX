using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

namespace ChatXService
{
    public class RabbitMQDriver : IMQDriver
    {
        //TODO: move these consts to a config file.
        private const string Q_CMD = "Command Queue";
        private const string Q_RESP = "Response Queue";

        private readonly Encoding Q_MSQ_ENC = Encoding.UTF8;

        private ConnectionFactory factory;

        public RabbitMQDriver(string hostname, string username, string password, string virtualHost)
        {
            factory = new ConnectionFactory();
            factory.HostName = hostname;
            /*
            factory.UserName = username;
            factory.Password = password;
            factory.VirtualHost = virtualHost;
             */
            ListenerOpen = false;
            SetupQRespListener(factory);

            //make sure OnResponseRecieved is not empty
            OnResponseRecieved += (cmd) => { };
        }

        public bool ListenerOpen { get; set; }

        private void SetupQRespListener(ConnectionFactory factory)
        {
            ListenerOpen = true;
            Thread responseThread = new Thread(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: Q_RESP,
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {


                            var body = ea.Body;
                            var message = Q_MSQ_ENC.GetString(body);

                            OnResponseRecieved(message);
                        };

                        channel.BasicConsume(queue: Q_RESP,
                                             noAck: true,
                                             consumer: consumer);

                        //TODO: keep connection open in a better way:
                        while (ListenerOpen)
                        {
                            Thread.Sleep(500);
                        }
                    }
                }
            });

            responseThread.Start();
        }

        public void SendCommand(string command)
        {
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {

                    byte[] msg = Q_MSQ_ENC.GetBytes(command);

                    channel.QueueDeclare(Q_CMD, true, false, false, null);
                    channel.BasicPublish("", "", null, msg);
                }
            }
        }

        public event CommandDelegate OnResponseRecieved;
    }
}
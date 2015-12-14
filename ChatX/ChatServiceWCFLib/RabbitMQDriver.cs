using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

            SetupQRespListener(factory);

            //make sure OnResponseRecieved is not empty
            OnResponseRecieved += (cmd) => { };
        }

        private void SetupQRespListener(ConnectionFactory factory)
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
                }
            }
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
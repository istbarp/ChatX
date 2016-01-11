using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatXTests
{
    [TestClass]
    public class RabbitMQTests
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        [TestInitialize]
        public void Init()
        {
            factory = new ConnectionFactory();
            factory.HostName = "localhost";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        [TestMethod]
        public void CommandQueue()
        {
            
            channel.QueueDeclare("Command Queue", true, false, false, null);
            channel.BasicPublish("", "Command Queue", null, Encoding.UTF8.GetBytes("this is a test"));
             

            Assert.IsTrue(false, "Test text send");
        }

        [TestCleanup]
        public void Cleanup()
        {
            channel.Close();
            connection.Close();
        }
    }
}

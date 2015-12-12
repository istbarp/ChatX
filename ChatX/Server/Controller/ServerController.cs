using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller
{
    class ServerController
    {
        private static ServerController instance;
        private IServerMQ queueManager;
        
        private readonly string responseQueueName = "Reponse Queue";

        private ServerController()
        {

        }

        public static ServerController GetInstance()
        {
            if (instance == null)
            {
                instance = new ServerController();    
            }
            
            return instance;
        }

        public void StartServer(string serviceIP, string hostName)
        {
            queueManager = new ServerRabbitMQ(hostName, responseQueueName);

            queueManager.OnCommandReceive += queueManager_OnCommandReceive;
        }

        void queueManager_OnCommandReceive(string command)
        {
            string[] cmd_parts = command.Split(':');

            switch (cmd_parts[0])
            {
                case "JOIN_ROOM":
                    JoinRoom(command);
                    break;
                case "LEAVE_ROOM":
                    LeaveRoom(command);
                    break;
                case "SEND_MESSAGE":
                    SendMessage(command);
                    break;
                case "REQUEST_ROOMS":
                    RequestRooms(command);
                    break;
                case "LOGIN_REQUEST":
                    LoginRequest(command);
                    break;
            }
           

            throw new NotImplementedException();
        }

        private void LoginRequest(string command)
        {
            throw new NotImplementedException();
        }

        private void RequestRooms(string command)
        {
            throw new NotImplementedException();
        }

        private void SendMessage(string command)
        {
            throw new NotImplementedException();
        }

        private void LeaveRoom(string command)
        {
            throw new NotImplementedException();
        }

        private void JoinRoom(string command)
        {
            throw new NotImplementedException();
        }

        public void StopServer()
        {

        }
    }
}

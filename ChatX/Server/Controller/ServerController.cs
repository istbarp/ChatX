using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Model;
using System.Net;
using Server.ChatSocket;
using ConfigDLL;

namespace Server.Controller
{
    class ServerController
    {
        private static ServerController instance;
        private IServerMQ queueManager;
        private Dictionary<User, IList<Room>> userRooms;
        private List<Room> allRooms;
        private List<User> allUsers;
        private List<User> lockedUsers;

        private Room lobby;

        private ServerController()
        {
            allRooms = new List<Room>();
            allUsers = new List<User>();
            userRooms = new Dictionary<User,IList<Room>>();
            lockedUsers = new List<User>();

            lobby = new Room("Lobby");

            allRooms.Add(lobby);
        }

        public static ServerController GetInstance()
        {
            if (instance == null)
            {
                instance = new ServerController();    
            }
            
            return instance;
        }

        public void StartServer(string localHostIP, string serviceIP)
        {
            queueManager = new ServerRabbitMQ(localHostIP, serviceIP);

            queueManager.OnCommandReceive += queueManager_OnCommandReceive;

            queueManager.StartListening();
        }

        void queueManager_OnCommandReceive(string command)
        {
            string[] cmd_parts = command.Split(Config.SEPERATOR);

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
                case "VAL_USERNAME":
                    VerifyAndLockUsername(command);
                    break;
                case "RELEASE_USERNAME":
                    ReleaseUsername(command);
                    break;
                default:
                    throw new Exception(String.Format("The command-type \"{0}\" was not recognized! \n in command:\n {1}", cmd_parts[0], command));
            }
        }

        private void ReleaseUsername(string command)
        {
            string id;
            string userName;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            id = cmd_parts[0];
            userName = cmd_parts[1];

            User user = new User(userName);

            lockedUsers.Remove(user);
        }

        private void VerifyAndLockUsername(string command)
        {
            string id;
            string userName;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            string response;

            id = cmd_parts[0];
            userName = cmd_parts[1];

            User cmdUser = new User(userName);

            if (lockedUsers.Contains(cmdUser) || allUsers.Contains(cmdUser))
            {
                response = "EXISTS";
            }
            else
            {
                lockedUsers.Add(cmdUser);
                response = "OK"; 
            }

            string cmdResp = Config.GenerateCommand(Config.CMD.VAL_USERNAME_REPONSE, id, response);

            queueManager.SendToResponseQueue(cmdResp);
        }

        //TO DO:ChatSocket
        private void LoginRequest(string command)
        {
            string id;
            string userName;
            string serverIP;
            string clientIP;

            string message;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            id = cmd_parts[1];
            userName = cmd_parts[2];
            serverIP = cmd_parts[3];
            clientIP = cmd_parts[4];

            if (Dns.GetHostEntry(Dns.GetHostName()).AddressList.Contains(IPAddress.Parse(serverIP)) || serverIP.Contains("127.0.0.1") || serverIP.Contains("localhost"))
            {
                User user = new User(userName, clientIP);

                //TODO: Open socket so user can connect.

                ConnectionManager conMan = ConnectionManager.GetInstance();
                IChatConnection chatCon = conMan.Listen(clientIP);

                user.ChatConnection = chatCon;

                IList<Room> defaultRooms = new List<Room>();
                defaultRooms.Add(lobby);

                userRooms.Add(user, defaultRooms);

                message = Config.GenerateCommand(Config.CMD.LOGIN_RESPONSE, id, "OK");
                queueManager.SendToResponseQueue(message);
            }
        }

        private void RequestRooms(string command)
        {
            string id;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            id = cmd_parts[0];

            string respVal = "";

            for (int i = 0; i < allRooms.Count; i++)
            {
                respVal += Config.SEPERATOR + allRooms[i].RoomName;
            }

            string response = Config.GenerateCommand(Config.CMD.REQUEST_ROOMS_RESPONSE, id, respVal);
            queueManager.SendToResponseQueue(response);
        }

        private void SendMessage(string command)
        {
            string id;
            string userName;
            string roomName;
            string message;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            //0 = command name
            id = cmd_parts[1];
            userName = cmd_parts[2];
            roomName = cmd_parts[3];

            //int paramLength = userName.Length + roomName.Length + id.Length + 3;
            message = cmd_parts[4];

            User sender = allUsers[allUsers.IndexOf(new User(userName))];

            Room[] rooms = userRooms[sender].ToArray();

            foreach (Room room in rooms)
            {
                foreach (User roomUser in room.Users)
                {
                    IChatConnection chatCon = roomUser.ChatConnection;
                    if (chatCon != null)
                    {
                        string chatMsg = String.Format("{0}|{1}|{2}", roomName, userName, message);
                        chatCon.SendMessage(chatMsg);
                    }
                }
            }
        }

        private void LeaveRoom(string command)
        {
            string id;
            string userName;
            string roomName;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            id = cmd_parts[0];
            userName = cmd_parts[1];
            roomName = cmd_parts[2];
        }

        private void JoinRoom(string command)
        {
            string id;
            string userName;
            string roomName;

            string response;

            string[] cmd_parts = command.Split(Config.SEPERATOR);

            id = cmd_parts[0];
            userName = cmd_parts[1];
            roomName = cmd_parts[2];

            User user = new User(userName);
            Room room = new Room(roomName);

            user = allUsers[allUsers.IndexOf(user)];

            if (allRooms.Contains(room))
            {
                room = allRooms[allRooms.IndexOf(room)];
            }
            else
            {
                allRooms.Add(room);
            }
            room.Users.Add(user);

            response = Config.GenerateCommand(Config.CMD.JOIN_ROOM_RESPONSE, id, "OK");
            queueManager.SendToResponseQueue(response);
        }

        public void StopServer()
        {
            queueManager.StopListening();
            queueManager = null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using RabbitMQ.Client;
using System.Threading;

namespace ChatXService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ChatXService : IChatXService
    {

        //TODO: move this to a "shared" config file between service and servers
        private readonly string[] CMD_FORMATS = {
                                                    "JOIN_ROOM:{0}:{1}:{2}",                //{0} = calling Thread ID,     {1} = username,    {2} = roomName
                                                    "JOIN_ROOM_RESPONSE:{0}:{1}:",          //{0} = requesting Thread ID,  {1} = OK | [ERROR]
                                                    "LEAVE_ROOM:{0}:{1}:{2}",               //{0} = calling Thread ID,     {1} = username,    {2} = roomName
                                                    "LEAVE_ROOM_RESPONSE:{0}:{1}:",         //{0} = requesting Thread ID,  {1} = OK | [ERROR]
                                                    "SEND_MESSAGE:{0}:{1}:{2}:{3}",         //{0} = calling Thread ID,     {1} = username,    {2} = roomName,     {3} = message
                                                    "REQUEST_ROOMS:{0}",                    //{0} = calling Thread ID
                                                    "REQUEST_ROOMS_RESPONSE:{0}:{n}",       //{0} = requesting Thread ID,  {n} = any number of roomNames seperated by ':'  
                                                    "LOGIN_REQUEST:{0}:{1}",                //{0} = calling Thread ID,     {1} = username  
                                                    "LOGIN_RESPONSE:{0}:{1}"                //{0} = calling Thread ID,     {1} = OK | [ERROR]
                                                };
        private enum CMD
        {
            /// <summary>
            /// Parameters: 
            /// <para> {0} = calling Thread ID  </para>
            /// <para> {1} = username    </para>
            /// <para> {2} = roomName </para>
            /// </summary>
            JOIN_ROOM = 0,

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            JOIN_ROOM_RESPONSE = 1,

            /// <summary>
            /// Parameters: 
            /// <para> {0} = calling Thread ID  </para>
            /// <para> {1} = username    </para>
            /// <para> {2} = roomName </para>
            /// </summary>
            LEAVE_ROOM = 2,

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            LEAVE_ROOM_RESPONSE = 3,

            /// <summary>
            /// Parameters: 
            /// <para> {0} = calling Thread ID  </para>
            /// <para> {1} = username    </para>
            /// <para> {2} = roomName </para>
            /// <para> {3} = message </para>
            /// </summary>
            SEND_MESSAGE = 4,

            /// <summary>
            /// Parameters:
            /// <para> {0} = calling Thread ID  </para>
            /// </summary>
            REQUEST_ROOMS = 5,

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {n} = any number of roomNames seperated by ':'  </para>
            /// </summary>
            REQUEST_ROOMS_RESPONSE = 6,

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = username  </para>
            /// </summary>
            LOGIN_REQUEST = 7,

            /// <summary>
            ///  Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            LOGIN_RESPONSE = 8

        }
        //END TODO

        private const int MAX_SLEEP_TIME = 30000;

        public void JoinRoom(string username, string roomName)
        {
            IMQDriver mqDriver = GetMQDriver();

            string command = GenerateCommand(CMD.JOIN_ROOM, Thread.CurrentThread.ManagedThreadId, username, roomName);
            bool hasResponded = false;

            mqDriver.OnResponseRecieved += (cmd) => {
                string[] cmdParts = cmd.Split(':');
                
                string cmdType = GetCmdType(CMD.JOIN_ROOM_RESPONSE);

                if (cmdParts[0].Equals(cmdType))
                {
                    if (cmdParts[1].Equals(Thread.CurrentThread.ManagedThreadId))
                    {
                        if (!cmdParts[2].ToUpper().Equals("OK"))
                        {
                            throw new FaultException(cmd);
                        }
                        else
                        {
                            hasResponded = true;
                        }
                    }
                }
            };

            Wait(hasResponded);

            mqDriver.SendCommand(command);
        }

        public void LeaveRoom(string username, string roomName)
        {
            IMQDriver mqDriver = GetMQDriver();

            string command = GenerateCommand(CMD.LEAVE_ROOM, Thread.CurrentThread.ManagedThreadId, username, roomName);
            bool hasResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                string[] cmdParts = cmd.Split(':');

                string cmdType = GetCmdType(CMD.LEAVE_ROOM_RESPONSE);

                if (cmdParts[0].Equals(cmdType))
                {
                    if (cmdParts[1].Equals(Thread.CurrentThread.ManagedThreadId))
                    {
                        if (!cmdParts[2].ToUpper().Equals("OK"))
                        {
                            throw new FaultException(cmd);
                        }
                        else
                        {
                            hasResponded = true;
                        }
                    }
                }
            };

            Wait(hasResponded);

            mqDriver.SendCommand(command);
        }

        public void SendMessage(string username, string roomName, string encryptedMessage)
        {
            IMQDriver mqDriver = GetMQDriver();

            string command = GenerateCommand(CMD.SEND_MESSAGE, Thread.CurrentThread.ManagedThreadId, username, roomName, encryptedMessage);

            mqDriver.SendCommand(command);
        }

        public string[] GetRooms()
        {
            IMQDriver mqDriver = GetMQDriver();
            string command = GenerateCommand(CMD.REQUEST_ROOMS, Thread.CurrentThread.ManagedThreadId);

            List<string> rooms = new List<string>();
            bool aServerHasResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                if (!aServerHasResponded)
                {
                    string cmdType = GetCmdType(CMD.REQUEST_ROOMS_RESPONSE);

                    string[] cmdParts = cmd.Split(':');

                    if (cmdParts[0].Equals(cmdType))
                    {
                        if (cmdParts[1].Equals(Thread.CurrentThread.ManagedThreadId))
                        {
                            for (int i = 2; i < cmdParts.Length; i++)
                            {
                                rooms.Add(cmdParts[i]);
                            }
                            aServerHasResponded = true;
                        }
                    }
                }
            };

            Wait(aServerHasResponded);

            return rooms.ToArray();
        }

        public string[] WhoIs(string roomName)
        {
            throw new NotImplementedException();
        }

        public string Login(string username)
        {
            IMQDriver mqDriver = GetMQDriver();
            string command = GenerateCommand(CMD.LOGIN_REQUEST, Thread.CurrentThread.ManagedThreadId, username);
            
            bool serverResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                string comType = GetCmdType(CMD.LOGIN_RESPONSE);
                string[] comParts = cmd.Split(':');

                if (comParts[0].Equals(comType))
                {
                    if (comParts[1].Equals(Thread.CurrentThread.ManagedThreadId))
                    {
                        if (comParts[2].ToUpper().Equals("OK"))
                        {
                            serverResponded = true;
                        }
                        else
                        {
                            throw new FaultException(cmd);
                        }
                    }
                }
            };

            Wait(serverResponded);

            return GetServerDestributor().RequestServer();
        }

        private IServerDestributer GetServerDestributor()
        {
            return new LocalServerDestributer();
        }

        private IMQDriver GetMQDriver()
        {
            IServerDestributer sd = GetServerDestributor();
            return sd.GetMQDriver(sd.RequestServer());
        }

        private string GenerateCommand(CMD cmd, params object[] args)
        {
            string command = "CMD_ERROR";

            string cmdFormat = CMD_FORMATS[(int)cmd];

            command = String.Format(cmdFormat, args);

            return command;
        }

        private string GetCmdType(CMD cmd)
        {
            return CMD_FORMATS[(int)cmd].Split(':')[0];
        }

        private void Wait(bool b)
        {
            int waitedTime = 0;
            while (!b && waitedTime < MAX_SLEEP_TIME)
            {
                Thread.Sleep(500);
                waitedTime += 500;
            }
        }
    }
}

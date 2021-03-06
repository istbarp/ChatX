﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RabbitMQ.Client;
using System.Threading;
using System.Web;
using System.ServiceModel.Web;

namespace ChatXService
{
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ChatXService : IChatXService
    {

        private const int MAX_SLEEP_TIME = 30000;

        public void JoinRoom(string username, string roomName)
        {
            IMQDriver mqDriver = GetMQDriver();

            string command = Config.GenerateCommand(Config.CMD.JOIN_ROOM, Thread.CurrentThread.ManagedThreadId, username, roomName);
            bool hasResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                string[] cmdParts = cmd.Split(':');

                string cmdType = GetCmdType(Config.CMD.JOIN_ROOM_RESPONSE);

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

            mqDriver.SendCommand(command);
            Wait(ref hasResponded);
            mqDriver.ListenerOpen = false;
        }

        public void LeaveRoom(string username, string roomName)
        {
            IMQDriver mqDriver = GetMQDriver();

            string command = Config.GenerateCommand(Config.CMD.LEAVE_ROOM, Thread.CurrentThread.ManagedThreadId, username, roomName);
            bool hasResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                string[] cmdParts = cmd.Split(':');

                string cmdType = GetCmdType(Config.CMD.LEAVE_ROOM_RESPONSE);

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

            mqDriver.SendCommand(command);
            Wait(ref hasResponded);
            mqDriver.ListenerOpen = false;
        }

        public void SendMessage(string username, string roomName, string encryptedMessage)
        {
            IMQDriver mqDriver = GetMQDriver();

            string command = Config.GenerateCommand(Config.CMD.SEND_MESSAGE, Thread.CurrentThread.ManagedThreadId, username, roomName, encryptedMessage);

            mqDriver.SendCommand(command);
            mqDriver.ListenerOpen = false;
        }

        public string[] GetRooms()
        {
            IMQDriver mqDriver = GetMQDriver();
            string command = Config.GenerateCommand(Config.CMD.REQUEST_ROOMS, Thread.CurrentThread.ManagedThreadId);

            List<string> rooms = new List<string>();
            bool aServerHasResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                if (!aServerHasResponded)
                {
                    string cmdType = GetCmdType(Config.CMD.REQUEST_ROOMS_RESPONSE);

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

            mqDriver.SendCommand(command);
            Wait(ref aServerHasResponded);
            mqDriver.ListenerOpen = false;

            return rooms.ToArray();
        }

        public string[] WhoIs(string roomName)
        {
            throw new NotImplementedException();
        }

        public string Login(string username)
        {
            //TODO make mqDriver into mqDriver for all servers

            IServerDestributer serverDist = new LocalServerDestributer();

            string reqServer = serverDist.RequestServer();
            IMQDriver mqDriver = serverDist.GetMQDriver(reqServer);

            LockUsername(username, mqDriver);

            //TODO make mqDriver into single server mqDriver
            string command = Config.GenerateCommand(Config.CMD.LOGIN_REQUEST, Thread.CurrentThread.ManagedThreadId, username, reqServer, GetClientIP());

            bool serverResponded = false;

            mqDriver.OnResponseRecieved += (cmd) =>
            {
                string comType = GetCmdType(Config.CMD.LOGIN_RESPONSE);
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

            Wait(ref serverResponded);

            ReleaseUsername(username, mqDriver);

            mqDriver.ListenerOpen = false;

            return GetServerDestributor().RequestServer();
        }

        private void LockUsername(string username, IMQDriver mqDriver)
        {
            string command = Config.GenerateCommand(Config.CMD.VAL_USERNAME, Thread.CurrentThread.ManagedThreadId, username);

            bool usernameLocked = false;
            mqDriver.OnResponseRecieved += (cmd) =>
            {
                string cmdType = GetCmdType(Config.CMD.VAL_USERNAME_REPONSE);
                string[] cmdParts = cmd.Split(Config.SEPERATOR);

                if (cmdParts[0].Equals(cmdType))
                {
                    if (cmdParts[1].Equals(Thread.CurrentThread.ManagedThreadId))
                    {
                        if (cmdParts[2].ToUpper().Equals("OK"))
                        {
                            usernameLocked = true;
                        }
                        else
                        {
                            throw new FaultException(cmd);
                        }
                    }
                }
            };
            mqDriver.SendCommand(command);
            Wait(ref usernameLocked);
        }

        private void ReleaseUsername(string username, IMQDriver mqDriver)
        {
            string command = Config.GenerateCommand(Config.CMD.RELEASE_USERNAME, Thread.CurrentThread.ManagedThreadId, username);
            mqDriver.SendCommand(command);

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

        private string GetCmdType(int cmd)
        {
            return Config.CMD_FORMATS[cmd].Split(Config.SEPERATOR)[0];
        }

        /// <summary>
        /// Waits as long as <paramref name="b"/> is false, or until aprox. MAX_SLEEP_TIME ms has passed
        /// </summary>
        /// <param name="b">The bool to wait on</param>
        private void Wait(ref bool b)
        {
            int waitedTime = 0;
            while (!b && waitedTime < MAX_SLEEP_TIME)
            {
                Thread.Sleep(500);
                waitedTime += 500;
            }
        }

        private string GetClientIP()
        {
            return "N/A";
            //return HttpContext.Current.Request.UserHostAddress;
        }
    }
}

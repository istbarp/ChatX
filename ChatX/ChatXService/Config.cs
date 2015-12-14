using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace ChatXService
{
    [DataContract]
    public class Config
    {
        [DataMember]
        public static readonly char SEPERATOR = '|';

        [DataMember]
        public static readonly string[] CMD_FORMATS = {
             //{0} = calling Thread ID,     {1} = username,    {2} = roomName
            "JOIN_ROOM" + SEPERATOR + "{0}" + SEPERATOR + "{1}" + SEPERATOR + "{2}",
                
            //{0} = requesting Thread ID,  {1} = OK | [ERROR]
            "JOIN_ROOM_RESPONSE" + SEPERATOR + "{0}" + SEPERATOR + "{1}",
            
             //{0} = calling Thread ID,     {1} = username,    {2} = roomName
            "LEAVE_ROOM" + SEPERATOR + "{0}" + SEPERATOR + "{1}" + SEPERATOR + "{2}",
            
            //{0} = requesting Thread ID,  {1} = OK | [ERROR]
            "LEAVE_ROOM_RESPONSE" + SEPERATOR + "{0}" + SEPERATOR + "{1}",
         
            //{0} = calling Thread ID,     {1} = username,    {2} = roomName,     {3} = message
            "SEND_MESSAGE" + SEPERATOR + "{0}" + SEPERATOR + "{1}" + SEPERATOR + "{2}" + SEPERATOR + "{3}",  
       
            //{0} = calling Thread ID
            "REQUEST_ROOMS" + SEPERATOR + "{0}",
            
            //{0} = requesting Thread ID,  {n} = any number of roomNames seperated by '" + SEPERATOR + "'  
            "REQUEST_ROOMS_RESPONSE" + SEPERATOR + "{0}" + SEPERATOR + "{n}",
       
            //{0} = calling Thread ID,     {1} = username     {2} = socketServerIP      {3} = clientIP
            "LOGIN_REQUEST" + SEPERATOR + "{0}" + SEPERATOR + "{1}" + SEPERATOR + "{2}" + SEPERATOR + "{3}",
            
            //{0} = requesting Thread ID,  {1} = OK | [ERROR]
            "LOGIN_RESPONSE" + SEPERATOR + "{0}" + SEPERATOR + "{1}",
            
            //{0} = calling Thread ID,     {1} = username
            "VAL_USERNAME" + SEPERATOR + "{0}" + SEPERATOR + "{1}",
            
             //{0} = requesting Thread ID,  {1} = OK | [ERROR]
            "VAL_USERNAME_REPONSE" + SEPERATOR + "{0}" + SEPERATOR + "{1}",         

            //{0} = calling Thread ID,     {1} = username,    {2} = OK | [ERROR]
            "RELEASE_USERNAME" + SEPERATOR + "{0}" + SEPERATOR + "{1}" + SEPERATOR + "{2}"          
        };

        [DataContract]
        public class CMD
        {
            /// <summary>
            /// Parameters: 
            /// <para> {0} = calling Thread ID  </para>
            /// <para> {1} = username    </para>
            /// <para> {2} = roomName </para>
            /// </summary>
            [DataMember]
            public static readonly int JOIN_ROOM = 0;

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            [DataMember]
            public static readonly int JOIN_ROOM_RESPONSE = 1;

            /// <summary>
            /// Parameters: 
            /// <para> {0} = calling Thread ID  </para>
            /// <para> {1} = username    </para>
            /// <para> {2} = roomName </para>
            /// </summary>
            [DataMember]
            public static readonly int LEAVE_ROOM = 2;

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            [DataMember]
            public static readonly int LEAVE_ROOM_RESPONSE = 3;

            /// <summary>
            /// Parameters: 
            /// <para> {0} = calling Thread ID  </para>
            /// <para> {1} = username    </para>
            /// <para> {2} = roomName </para>
            /// <para> {3} = message </para>
            /// </summary>
            [DataMember]
            public static readonly int SEND_MESSAGE = 4;

            /// <summary>
            /// Parameters:
            /// <para> {0} = calling Thread ID  </para>
            /// </summary>
            [DataMember]
            public static readonly int REQUEST_ROOMS = 5;

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {n} = any number of roomNames seperated by SEPERATOR  </para>
            /// </summary>
            [DataMember]
            public static readonly int REQUEST_ROOMS_RESPONSE = 6;

            /// <summary>
            /// Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = username  </para>
            /// <para> {2} = Socket server IP</para>
            /// <para> {3} = Client IP</para>
            /// </summary>
            [DataMember]
            public static readonly int LOGIN_REQUEST = 7;

            /// <summary>
            ///  Parameters:
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            [DataMember]
            public static readonly int LOGIN_RESPONSE = 8;

            /// <summary>
            /// Verify and lock username
            /// <para> Parameters: </para>
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = username </para>
            /// </summary>
            [DataMember]
            public static readonly int VAL_USERNAME = 9;

            /// <summary>
            /// Verify and lock username
            /// <para> Parameters: </para>
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            [DataMember]
            public static readonly int VAL_USERNAME_REPONSE = 10;

            /// <summary>
            /// <para> {0} = requesting Thread ID  </para>
            /// <para> {1} = OK | [ERROR]    </para>
            /// </summary>
            [DataMember]
            public static readonly int RELEASE_USERNAME = 11;
        }

        public static string GenerateCommand(int cmd, params object[] args)
        {
            string command = "CMD_ERROR";

            string cmdFormat = Config.CMD_FORMATS[cmd];

            command = String.Format(cmdFormat, args);

            return command;
        }
    }
}
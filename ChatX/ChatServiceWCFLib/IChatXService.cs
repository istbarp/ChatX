using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatXService
{
    //TODO: Agree on returned format and encoding

    [ServiceContract]
    public interface IChatXService
    {
        /// <summary>
        /// Joins <paramref name="roomName"/> if it exists. If not, then creates the room and THEN joins it.
        /// </summary>
        /// <args name="username">The name of the user who wishes to join</args>
        /// <args name="roomName">The name of the room to be joined/created</args>
        [OperationContract]
        void JoinRoom(string username, string roomName);

        /// <summary>
        /// Makes <paramref name="username"/> leave <paramref name="roomName"/>.
        /// </summary>
        /// <args name="username">The user who wishes to leave</args>
        /// <args name="roomName">The room, which <paramref name="username"/> wants to leave</args>
        [OperationContract]
        void LeaveRoom(string username, string roomName);

        /// <summary>
        /// Sends a message from <paramref name="username"/> to <paramref name="roomName"/>.
        /// The message should be encrypted using the public key gotten through the socket-connection
        /// established after Login.
        /// </summary>
        /// <args name="username">The user who sends the message</args>
        /// <args name="roomName">The name of the room to which the message is to be send</args>
        /// <args name="encryptedMessage">The message to be send to <paramref name="roomName"/></args>
        [OperationContract]
        void SendMessage(string username, string roomName, string encryptedMessage);

        /// <summary>
        /// Requests a list of all availble rooms to join.
        /// </summary>
        [OperationContract]
        string[] GetRooms();

        /// <summary>
        /// Requests an array of usernames connected to the specified room.
        /// </summary>
        /// <args name="roomName">The name of the room for which an array of users is wanted</args>
        [OperationContract]
        string[] WhoIs(string roomName);

        /// <summary>
        /// Requests a string with information, which can be used to make a socket-connection to a server.
        /// This socket-connection will then be used to respond to any further requests made to this service.
        /// It will respond with "USERNAME_ACCEPTED" or "USERNAME_DENIED" encoded in ASCII.
        /// </summary>
        /// <args name="username">The requested username</args>
        /// <returns>A string with the format [ip]:[port]. NOTE this IP can be IPv4 or IPv6</returns>
        [OperationContract]
        string Login(string username);
    }
}

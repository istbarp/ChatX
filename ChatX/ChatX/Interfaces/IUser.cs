using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatX.Interfaces
{
    interface IUser
    {
        /// <summary>
        /// Retrieves an array of users connected to this room.
        /// </summary>
        /// <returns>An array of IUser objects.</returns>
        IUser[] ListUsers();

        /// <summary>
        /// Connects a user to this room.
        /// </summary>
        /// <param name="user">The user who wishes to connect</param>
        void Connect(IUser user);

        /// <summary>
        /// Removes a user from this room.
        /// </summary>
        /// <param name="user">The user to be removed</param>
        void Disconnect(IUser user);
    }

}

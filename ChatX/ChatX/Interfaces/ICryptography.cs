using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatX.Interfaces
{
    interface ICryptography
    {
        byte[] Encrypt(IMessage msg);
        IMessage Decrypt(byte[] encMsg);
        void GenerateKeyPairs();
    }
}

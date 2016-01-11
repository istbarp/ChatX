using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.ChatSocket
{
    public delegate void MessageSend(string message);

    public class ConnectionManager
    {
        private readonly int PORT = 9966;
        private readonly int LISTEN_BACKLOG = 10;

        private Socket listener;
        private static ConnectionManager instance;

        private List<string> acceptedIPs;
        private Dictionary<string, Socket> ipToSocket;

        public event MessageSend OnMessageSend;

        private ConnectionManager()
        {
            acceptedIPs = new List<string>();
            ipToSocket = new Dictionary<string, Socket>();

            //TODO: refactor
            /*
            IPHostEntry entity = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress ip = entity.AddressList[0];

            IP = ip.ToString();
            Port = PORT;
            */

            IP = "127.0.0.1";
            Port = PORT;

            listener = new Socket(SocketType.Stream, ProtocolType.IP);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
            listener.Bind(endPoint);

            OnMessageSend += (msg) => { };
        }

        public string IP { get; private set; }
        public int Port { get; set; }

        public static ConnectionManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ConnectionManager();
            }

            return instance;
        }

        public IChatConnection Listen(string remoteAddress)
        {
            Socket handler = null;
            if (acceptedIPs.Contains(remoteAddress))
            {
                handler = ipToSocket[remoteAddress];
            }
            else
            {
                listener.Listen(LISTEN_BACKLOG);
                Socket acceptedSocket = listener.Accept();

                IPEndPoint remoteIpEndp = acceptedSocket.RemoteEndPoint as IPEndPoint;

                if (remoteIpEndp.Address.ToString().Equals(remoteAddress))
                {
                    handler = acceptedSocket;
                }
                else
                {
                    acceptedIPs.Add(remoteAddress);
                    ipToSocket.Add(remoteAddress, acceptedSocket);
                }
            }
            IChatConnection chatCon = new ChatConnection(handler);
            chatCon.MessageSendListeners += (msg) => { OnMessageSend(msg);  };

            return new ChatConnection(handler);
        }
    }
}

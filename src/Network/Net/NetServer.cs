using System;
using System.Net.Sockets;
using System.Net;
using Network.Log;

namespace Network.Net
{
    public delegate void OnAcceptedHandler(NetServerLink link);
    public class NetServer
    {
        protected string ip;
        protected ushort port;
        protected ILinkListenner listenner;
        protected bool reusable;
        protected bool needCheckValid;
        protected Socket socket;
        protected OnAcceptedHandler acceptedHandler;
        protected OnMessageHandler messageHandler;

        public NetServer(string ip, ushort port,
            ILinkListenner listenner,
            bool reusable, bool needCheckValid,
            OnAcceptedHandler acceptedHandler,
            OnMessageHandler messageHandler)
        {
            this.ip = ip;
            this.port = port;
            this.listenner = listenner;
            this.reusable = reusable;
            this.needCheckValid = needCheckValid;
            this.acceptedHandler = acceptedHandler;
            this.messageHandler = messageHandler;
        }

        public bool Start()
        {
            try
            {
                IPAddress ipa = IPAddress.Parse(ip);
                socket = new Socket(ipa.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iep = new IPEndPoint(ipa, port);
                socket.Bind(iep);
                socket.Listen(5);
                socket.BeginAccept(OnAccepted, socket);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogDebug("NetServer:Start:Error: " + ex.Message);
                return false;
            }
            return true;
        }

        public void Stop()
        {
            socket.Close();
        }

        protected void OnAccepted(IAsyncResult ar)
        {
            try
            {
                if (socket == null)
                    return;
                Socket newSocket = socket.EndAccept(ar);
                NetServerLink link = new NetServerLink(newSocket, reusable, needCheckValid);
                link.PushFlagPacket(FlagPacketTypes.FPT_CONNECT_OK);
                link.MsgHandler = messageHandler;
                link.Listenners.Add(listenner);
                acceptedHandler(link);
                socket.BeginAccept(OnAccepted, socket);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogDebug("NetServer:OnAccepted:Error: " + ex.Message);
            }
        }
    }
}

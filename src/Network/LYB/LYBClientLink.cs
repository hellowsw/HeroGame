using Network.Timer;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace Network.Net
{
    // net interface
    public class CppNetLYB
    {
#if UNITY_IPHONE
        public const string LUADLL = "__Internal";
#else
        public const string LUADLL = "hzdcore";
#endif

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Initialize();

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Connect(IntPtr clsPoint, String szIP, String szPortName, bool bBlock, int timeoutTime);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Disconnect(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Send(IntPtr clsPoint, IntPtr pvBuf, ushort wSize);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Get(IntPtr clsPoint, out IntPtr ppPacket);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetNetVersion(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetDebug(IntPtr clsPoint, bool dbg);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsConnected(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool KeepAlive(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPing(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPrevPongTime(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSrvTime(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPacketNumber(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetOption(IntPtr clsPoint, uint param);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsWaitingConnect(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Close(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ReleasePacket(IntPtr clsPoint, IntPtr pPacket);
    }

    // data serializer
    public class HzDSerializer
    {
#if UNITY_IPHONE
        public const string LUADLL = "__Internal";
#else
        public const string LUADLL = "hzdcore";
#endif
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Serializer(IntPtr data, uint size);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int curSize(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int maxSize(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curAddr(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int EndEdition(IntPtr clsPoint);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PushVariant(IntPtr clsPoint, IntPtr data, uint size, uint dataType);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadEnd(IntPtr clsPoint);
    }
    public class LYBNetDefs
    {
        // ping消息发送间隔
        public const uint PINGMARGIN = 15000;

        // "ping"的ASCII码
        public const uint PING = 0x676e6970;

        // "pong"的ASCII码 
        public const uint PONG = 0x676e6f70;

        // "tick"的ASCII码
        public const uint TICK = 0x6b636974;
    }

    public static class IntervalOutput
    {
        static UInt64 outputTick = TickerPolicy.SysTicker.GetTick();
        static bool IntervalTest()
        {
            var now = TickerPolicy.SysTicker.GetTick();
            if (now < outputTick)
                return false;
            outputTick = now + 6000;
            return true;
        }
        public static void Log(string fmt, params object[] parameters)
        {
            if (!IntervalTest())
                return;
            Debugger.Log(fmt, parameters);
        }
        public static void LogWarning(string fmt, params object[] parameters)
        {
            if (!IntervalTest())
                return;
            Debugger.LogWarning(fmt, parameters);
        }
        public static void LogError(string fmt, params object[] parameters)
        {
            if (!IntervalTest())
                return;
            Debugger.LogError(fmt, parameters);
        }
    }

    public enum DisconnectReason
    {
        Active,         //主动断开
        Normal,         //连接正常断开
        Timeout,        //连接超时断开
    }
    public class LYBClientLink
    {
        public OnMessageHandler MsgHandler;
        public OnMessageHandler SpecMsgHandler;

        private IntPtr clientNet = IntPtr.Zero;
        public List<ILinkListenner> Listenners = new List<ILinkListenner>();
        UInt64 linkID;
        public UInt64 LinkID { get { return linkID; } }

        // 统计x秒内的消息数
        private int second = 60;
        private bool statisticsMessage = true;
        private int lastStartTime = System.Environment.TickCount;
        private int messageCount = 0;
        private int maxMessageCount = 0;
        public bool StatisticsMessage { get { return statisticsMessage; } }
        public string MessageCountText { get { return second + "秒最大消息数量:" + maxMessageCount; } }
        public string MessageCurCountText { get { return second + "秒:" + messageCount + "(" + ((System.Environment.TickCount - lastStartTime) / 1000) + ")"; } }

        public LYBClientLink(ILinkListenner listenner)
        {
            clientNet = CppNetLYB.Initialize();
            if (clientNet == IntPtr.Zero)
                throw new Exception("HzDNet init error !");

            CppNetLYB.SetDebug(clientNet, false);

            linkID = NetLinkBase.GenLinkID();
            Listenners.Add(listenner);
            IntPtr version = CppNetLYB.GetNetVersion(clientNet);
            Debugger.Log("HzDNet version:{0}, clientNet:{1}", Marshal.PtrToStringAnsi(version), clientNet);
            Debugger.Log("LYBClientLink Constructor LinkId:{0}", linkID);
        }

        public bool Connect(string ip, string port)
        {
            // 第4个参数表示等待的超时时间，设置为-1表示不超时，具体参数需要实测
            return 1 == CppNetLYB.Connect(clientNet, ip, port, false, 6);
        }

        public bool IsConnected()
        {
            return ((clientNet != IntPtr.Zero) && (CppNetLYB.IsConnected(clientNet)));
        }

        public int GetPing()
        {
            if (IsConnected()) return CppNetLYB.GetPing(clientNet);
            return -1;
        }

        public int GetServerTime()
        {
            if (IsConnected()) return CppNetLYB.GetSrvTime(clientNet);
            return -1;
        }
        public bool IsWaitingConnect()
        {
            return CppNetLYB.IsWaitingConnect(clientNet);
        }

        public void Disconnect()
        {
            Disconnect(DisconnectReason.Active);
        }

        DisconnectReason lastDisconnectReason = DisconnectReason.Active;
        public DisconnectReason GetDisconnectReason() { return lastDisconnectReason; }
        public void Disconnect(DisconnectReason reason)
        {
            if (clientNet == IntPtr.Zero)
                return;
            CppNetLYB.Disconnect(clientNet);
            Release();
            if (aliveThread != null)
                aliveThread.Join();

            Debugger.Log(string.Format("HzDNet Disconnect：{0},LinkId:{1}", reason.ToString(), LinkID));

            lastDisconnectReason = reason;
            if (reason != DisconnectReason.Active)
                OnDisconnect();
        }

        public void Release()
        {
            if (clientNet == IntPtr.Zero)
                return;
            CppNetLYB.Close(clientNet);
            clientNet = IntPtr.Zero;
        }

        public bool Send(byte[] datas)
        {
            return Send(datas, datas.Length);
        }

        public unsafe bool Send(byte[] datas, int dataSize)
        {
            if (!IsConnected())
                return false;
            Debugger.LogWarning("send data " + datas[0] + "\t" + datas[1] + "\t" + datas[2] + "\t" + datas[3] + "\t" + datas[4]);
            fixed (byte* pt = &datas[0])
            {
                IntPtr pObject = (IntPtr)pt;
                int result = CppNetLYB.Send(clientNet, pObject, (ushort)dataSize);
                if (result <= 0)
                {
                    Debugger.LogError("HzDNet send error: " + result);
                    return false;
                }
                Debugger.LogWarning("Send datas size: " + dataSize);
                if (statisticsMessage)
                {
                    if (System.Environment.TickCount - lastStartTime >= second * 1000)
                    {
                        lastStartTime = System.Environment.TickCount;
                        maxMessageCount = Math.Max(maxMessageCount, messageCount);
                        messageCount = 0;
                    }
                    else
                    {
                        messageCount++;
                    }
                }
            }
            return true;
        }

        //LYB限制每15秒发一个, 客户端隔一个Ping请求进行实际发送
        //20161019版本更新, 间隔范围为(1500 - 15000), 每Ping发送
        int sendAliveDelay = 3000;
        UInt64 lastAliveSendTick = 0;
        TMemoryBufferEx packetBuff = new TMemoryBufferEx();

        Thread aliveThread = null;
        void AliveThread()
        {
            while (clientNet != IntPtr.Zero)
            {
                UInt64 nowTick = TickerPolicy.SysTicker.GetTick();
                if ((int)(nowTick - lastAliveSendTick) > sendAliveDelay)
                {
                    lastAliveSendTick = nowTick;
                    CppNetLYB.KeepAlive(clientNet);
                    Debugger.LogWarning("alive prev pong = {0}, ping = {1}", CppNetLYB.GetPrevPongTime(clientNet), CppNetLYB.GetPing(clientNet));
                }
                Thread.Sleep(5);
            }
        }

        public unsafe void Execute()
        {
            if (clientNet == IntPtr.Zero)
                return;

            //AliveThread();

            // 获取距离上次收到pong的时间差值，如果这个差值过大（大于60秒）则表示这么长时间内未收到服务器的应答，可以考虑断开重连（需实测）
            int prevPong = CppNetLYB.GetPrevPongTime(clientNet);
            // IntervalOutput.Log("PrevPong Interval: {0}", prevPong);
            if (prevPong > 100000)
            {
                Disconnect(DisconnectReason.Timeout);
                return;
            }

            IntPtr dataPtr;
            while (CppNetLYB.Get(clientNet, out dataPtr) > 0)
            {
                SNetClientPacket packet = (SNetClientPacket)Marshal.PtrToStructure(dataPtr, typeof(SNetClientPacket));
                Debugger.LogWarning("Recv packet , len = " + packet.m_wLength);

                if (packet.m_wLength == 0)//服务器断开连接//
                {
                    Disconnect(DisconnectReason.Normal);
                    return;
                }
                else if (packet.m_wLength == 1)
                {
                    bool bConnected = CppNetLYB.IsConnected(clientNet);
                    OnConnected(bConnected);
                }
                else
                {
                    //packetBuff.Reset();
                    //packetBuff.WriteUnsafeBuff((byte*)packet.m_pvBuffer, 0, packet.m_wLength);

                    //if (packet.m_wLength == 8) // 可能是"pong"或"tick"
                    //{
                    //    var key = *((uint*)packet.m_pvBuffer);
                    //    if (key == LYBNetDefs.PONG ||
                    //        key == LYBNetDefs.PING ||
                    //        key == LYBNetDefs.TICK)
                    //    {
                    //        SpecMsgHandler?.Invoke(linkID, packetBuff);
                    //        return;
                    //    }
                    //}
                    //MsgHandler(linkID, packetBuff);
                    packetBuff.Reset();
                    packetBuff.WriteUnsafeBuff((byte*)packet.m_pvBuffer, 0, packet.m_wLength);
                    MsgHandler(linkID, packetBuff);
                }

                CppNetLYB.ReleasePacket(clientNet, dataPtr);
            }

           
        }

        protected void OnConnected(bool isOk)
        {
            if (isOk && aliveThread == null)
            {
                aliveThread = new Thread(AliveThread);
                aliveThread.Start();
            }

            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnConnected(linkID, isOk);
            }
        }

        protected void OnDisconnect()
        {
            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnDisconnected(linkID);
            }
        }

        #region LinkMsgHelper
        protected LYBMsgSerializerIn arIn = new LYBMsgSerializerIn();
        public bool Send<T>(T msg) where T : LYBMsgHeader
        {
            msg.Serialize(arIn);
            bool ret = Send(arIn.Buff, arIn.Size);
            arIn.Reset();
            arIn.ResetStream();
            return ret;
        }

        #endregion
    }
}

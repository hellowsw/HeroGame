using Network.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Net
{
    public class MsgPool : ObjectPool
    {
        private static MsgPool instance;
        public static MsgPool Instance
        {
            get
            {
                if (instance == null)
                    instance = new MsgPool();
                return instance;
            }
        }
    }

    public class MsgDispatcher : Singleton<MsgDispatcher>
    {
        Dictionary<uint, OnMessageHandler> handlers = new Dictionary<uint, OnMessageHandler>();

        public void RegMsgHandler(uint opCode, OnMessageHandler handler)
        {
            handlers[opCode] = handler;
        }

        OnMessageHandler GetMsgHandler(uint opCode)
        {
            OnMessageHandler ret = null;
            handlers.TryGetValue(opCode, out ret);
            return ret;
        }

        public void OnMessage(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            OnMessageHandler handler = GetMsgHandler(MsgHeader.GetDataOpCode(packetBuff));
            if (handler != null)
                handler(linkID, packetBuff);
        }

        public void Clear()
        {
            handlers.Clear();
        }
    }

    public class LYBMsgDispatcher : Singleton<LYBMsgDispatcher>
    {
        Dictionary<uint, OnMessageHandler> handlers = new Dictionary<uint, OnMessageHandler>();

        public void RegMsgHandler(uint opCode, OnMessageHandler handler)
        {
            handlers[opCode] = handler;
        }

        OnMessageHandler GetMsgHandler(uint opCode)
        {
            OnMessageHandler ret = null;
            handlers.TryGetValue(opCode, out ret);
            return ret;
        }


        static LYBMsgHeader headerAssistor = new LYBMsgHeader();
        public static uint GetDataOpCode(TMemoryBufferEx packetBuff)
        {
            arOut.Reset(packetBuff.GetBuffer(), packetBuff.ReadPos, packetBuff.Size);
            headerAssistor.Serialize(arOut);
            return headerAssistor.GetOpCode();
        }

        protected static LYBMsgSerializerOut arOut = new LYBMsgSerializerOut();
        protected static LYBMsgHeader processingMsg = null;
        public static T GetMsg<T>(TMemoryBufferEx packetBuff, bool show = true) where T : LYBMsgHeader, new()
        {
            T msg = null;
            GetMsg(packetBuff, ref msg, show);
            return msg;
        }

        public static bool GetMsg<T>(TMemoryBufferEx packetBuff, ref T msg, bool show = true) where T : LYBMsgHeader, new()
        {
            try
            {
                if (msg == null)
                {
                    processingMsg = msg = MsgPool.Instance.Get<T>();
                }
                arOut.ResetStream();
                arOut.Reset(packetBuff.GetBuffer(), packetBuff.ReadPos, packetBuff.Size);
                msg.Serialize(arOut);

                if (show)
                {
                    Debugger.Log("GetMsg=========:     " + typeof(T).ToString());
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Debugger.LogError("NetManager:GetMsg:Error:" + ex.Message);
                throw ex;
            }
        }

        public delegate void UnhandledMsgHandlerDecl(byte firstID, byte secondID, TMemoryBufferEx onePacketBuff);
        public UnhandledMsgHandlerDecl UnhandledMsgHandler = null;
        public void OnMessage(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            arOut.Reset(packetBuff.GetBuffer(), packetBuff.ReadPos, packetBuff.Size);
            headerAssistor.Serialize(arOut);

            OnMessageHandler handler = GetMsgHandler(GetDataOpCode(packetBuff));
            if (handler != null)
            {
                handler(linkID, packetBuff);
            }
            else if (UnhandledMsgHandler != null)
            {
                UnhandledMsgHandler(headerAssistor.firstProtol, headerAssistor.secondProtol, packetBuff);
            }
            else
            {
                Debugger.Log("LYBMessage:{0} {1} 没有对应的处理函数", headerAssistor.firstProtol, headerAssistor.secondProtol);
            }

            if (processingMsg != null)
            {
                MsgPool.Instance.Free(processingMsg);
                processingMsg = null;
            }
        }

        public void OnSpecMessage(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            var key = packetBuff.ReadUInt();
            packetBuff.ReadPos -= sizeof(uint);
            OnMessageHandler handler = GetMsgHandler(key);
            if (handler != null)
                handler(linkID, packetBuff);
        }
        
        public void Clear()
        {
            handlers.Clear();
        }
    }
}

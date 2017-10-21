using Network.Compress;
using Network.Timer;
using System;
using System.Net.Sockets;
using Network.Log;
using System.Net;
using System.Collections.Generic;

namespace Network.Net
{
    public delegate void OnMessageHandler(UInt64 linkID, TMemoryBufferEx onePacketBuff);
    public class NetLinkBase
    {
        protected static UInt64 linkIDBase = 0;
        public static UInt64 GenLinkID() { return ++linkIDBase; }

        protected UInt64 linkID;
        protected UInt64 sessionKey;
        protected Socket socket;
        protected RecvBuff recvBuff = new RecvBuff();
        protected bool isConnected = false;
        protected bool isWaitingClose = false;
        protected bool isEncrypt = true;
        protected bool isCompress = true;//临时不压缩
        protected const int minCompressSize = 512;
        protected ushort recvSeqNoBase = 0, sendSeqNoBase = 0;
        protected RC6 sendCrypter = new RC6();
        protected RC6 recvCrypter = new RC6();
        protected SerializeArchiveIn arIn = new SerializeArchiveIn();

        protected TMemoryBufferEx processingBuffer = new TMemoryBufferEx(128 * 1024);   //处理压缩与解密时与sendBuffer进行引用交换,处理完成后将立即Reset
        protected TMemoryBufferEx sendBuffer = new TMemoryBufferEx(128 * 1024);

        protected object sendBufferLock = new object();
        protected bool sendingFlag = false;

        protected TMemoryBufferEx sendingBuffer = new TMemoryBufferEx(128 * 1024);
        protected bool reusable = false;
        protected bool needCheckValid = false;
        public bool Reusable
        {
            get { return reusable; }
        }

        public UInt64 LinkID
        {
            get { return linkID; }
        }

        public UInt64 SessionKey
        {
            get { return sessionKey; }
        }

        public static bool NeedKeepAlive = true;

        public NetLinkBase(bool reusable, bool needCheckValid)
        {
            this.linkID = GenLinkID();
            this.reusable = reusable;
            this.needCheckValid = needCheckValid;
            this.sessionKey = 0;

            if (reusable)
            {
                resendBuffer = new TMemoryBufferEx(1024 * 1024);
                resendBuffSizeOnPing = new TMemoryBufferEx(4 * 128);
            }
        }

        public static int RecvAliveDelay = 25000;
        public static int RecvAliveDelayOnReusing = 35000;
        public static int SendAliveDelay = 3000;
        protected UInt64 lastAliveSendTick = 0;
        protected UInt64 lastAliveRecvTick = 0;

        protected void KeepAlive()
        {
            if (linkStatus != LinkStatus.CONNECTED || reusing)
                return;

            UInt64 nowTick = TickerPolicy.Ticker.GetTick();
            if ((int)(nowTick - lastAliveSendTick) < SendAliveDelay)
                return;
            
            lastAliveSendTick = nowTick;
            SendPing();
            // LogManager.Instance.LogDebug(nowTick + ":" + linkID + ":KeepAlive:" + lastAliveSendTick);
        }

        public void CheckAlive()
        {
            if (linkStatus != LinkStatus.CONNECTED || !isConnected)
                return;

            // 仅加长而不避免重用状态下的连接超时检测
            int checkDelay = RecvAliveDelay;
            if (reusing)
                checkDelay = RecvAliveDelayOnReusing;

            UInt64 nowTick = TickerPolicy.Ticker.GetTick();
            if ((int)(nowTick - lastAliveRecvTick) > checkDelay)
            {
                Close();
                LogManager.Instance.LogWarning(nowTick + ":" + linkID + "连接超时关闭, 包处理总次数: " + packetHandleCount + "\r\n");
            }
        }

        public bool Send<T>(T msg) where T : MsgHeader
        {
            msg.Serialize(arIn);
            bool ret = Send(arIn.Buff, arIn.Size);
            arIn.Reset();
            return ret;
        }

        public bool Send(byte[] buff, int dataSize)
        {
            return Send(buff, dataSize, 0);
        }

        protected int resendDataSizeOnPing = 0;
        public bool Send(byte[] buff, int dataSize, uint sendPingSeqNo)
        {
            if (isWaitingClose)
                return false;

            lock (sendBufferLock)
            {
                int headerSize = 0;
                unsafe
                {
                    // 填充包头
                    PacketHeader header;
                    headerSize = sizeof(PacketHeader);

                    header.bodyLen = dataSize;
                    header.seqNo = ++sendSeqNoBase;
                    header.flag = 0;

                    if (isEncrypt)
                        header.flag |= FlagCode.FLAG_CRYPTED;
                    if (isCompress && dataSize > minCompressSize)
                        header.flag |= FlagCode.FLAG_COMPRESSED;

                    //LogManager.Instance.LogDebug("------->包, 长度: {0} 序列号:{1} ,正确序号:{2}", header.bodyLen, header.seqNo, sendSeqNoBase);

                    byte* headerPtr = (byte*)&header;
                    sendBuffer.WriteUnsafeBuff(headerPtr, 0, headerSize);
                    sendBuffer.Write(buff, 0, dataSize);

                    if (reusable)
                    {
                        //CheckResendBuff("Send0");
                        // 重发数据不压缩与加密
                        header.flag = 0;
                        resendBuffer.WriteUnsafeBuff(headerPtr, 0, headerSize);
                        resendBuffer.Write(buff, 0, dataSize);
                        resendDataSizeOnPing += (headerSize + dataSize);
                        //CheckResendBuff("Send1");

                        if (sendPingSeqNo > 0)
                        {
                            //DebugHelper.LogWarning("WriteInt:" + resendDataSizeOnPing);
                            resendBuffSizeOnPing.WriteInt(resendDataSizeOnPing);
                            resendBuffSizeOnPing.WriteUInt64(TickerPolicy.Ticker.GetTick());
                            resendDataSizeOnPing = 0;
                        }
                    }
                }

                if (!sendValid || !isConnected)
                    return true;
            }
            OnSendRequest();
            return true;
        }
        
        protected TMemoryBufferEx resendBuffer;
        protected uint pingSeqNo = 0;

        protected TMemoryBufferEx resendBuffSizeOnPing;
        protected int resendBuffMaxPreSize = 2048;
        protected byte[] alivePingData = new byte[8];
        protected byte[] alivePongData = new byte[8];

        protected unsafe void SendPing()
        {
            fixed (byte* ptr = &alivePingData[0])
            {
                ((uint*)ptr)[0] = 0xecceccff;
                ((uint*)ptr)[1] = ++pingSeqNo;
            }
            Send(alivePingData, alivePingData.Length, pingSeqNo);
        }

        protected unsafe void SendPong(uint pingSeq)
        {
            fixed (byte* ptr = &alivePongData[0])
            {
                ((uint*)ptr)[0] = 0xecceccfe;
                ((uint*)ptr)[1] = pingSeq;
            }
            Send(alivePongData, alivePongData.Length);
        }

        protected uint lastCollectedPingSeq = 0;
        protected UInt64 pingDelay = 0;
        public UInt64 GetPing() { return pingDelay; }
        protected void OnPong(ulong recvTick, uint pingSeq)
        {
            bool isErrorSeqNo = ((lastCollectedPingSeq + 1) != pingSeq);
            if (isErrorSeqNo)
            {
                LogManager.Instance.LogError("心跳错误: ping序号: {0}, 正确序号:{1}", pingSeq, lastCollectedPingSeq + 1);
                LogManager.Instance.LogError((new System.Diagnostics.StackTrace()).ToString());
                //throw new Exception("心跳错误.");
            }
            lastCollectedPingSeq = pingSeq;

            if (!reusable || isErrorSeqNo)
                return;

            lock (sendBufferLock)
            {
                int collectSize = resendBuffSizeOnPing.ReadInt();
                UInt64 sendTick = resendBuffSizeOnPing.ReadUInt64();
                pingDelay = recvTick - sendTick;

                //DebugHelper.LogWarning("ReadInt:" + collectSize);
                if (collectSize > resendBuffer.Size)
                {
                    throw new Exception("缓冲回收错误.");
                }

                //CheckResendBuff("OnPong0");
                //int oldResendSize = resendBuffer.Size;
                resendBuffer.Read(null, 0, collectSize);
                //CheckResendBuff("OnPong1:" + collectSize + ":" + pingSeq + ":" + oldResendSize + ":" + resendBuffer.Size);

                int preSpaceSize = resendBuffer.ReadPos;
                if (preSpaceSize >= resendBuffMaxPreSize)
                {
                    resendBuffer.MoveDataToBegin();
                    //CheckResendBuff("OnPong2");
                }
            }
        }

        public void ForceDisconnect()
        {
            Close();
        }

        public void Disconnect()
        {
            if (!isConnected || linkStatus == LinkStatus.DISCONNECTED)
            {
                PushFlagPacket(FlagPacketTypes.FPT_DESTORY);
                return;
            }

            try
            {
                if (isWaitingClose)
                    return;

                isWaitingClose = true;

                lock (sendingLock)
                {
                    if (sendingFlag)
                        return;
                }

                // 通知发送队列处理关闭
                OnSendRequest();
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogError(ex.Message);
                LogManager.Instance.LogError(ex.StackTrace);
            }
        }

        public bool IsConnected()
        {
            return isConnected && !isWaitingClose && linkStatus == LinkStatus.CONNECTED;
        }

        protected void Close()
        {
            lock (socket)
            {
                if (!isConnected)
                    return;

                isConnected = false;
                socket.Close();
                sendValid = false;
                recvValid = false;
            }
        }

        protected bool sendValid = true;
        protected bool recvValid = true;

        object sendingLock = new object();
        int preparedSize = 0;
        protected TMemoryBufferEx compressBuff = new TMemoryBufferEx(16 * 1024);
        public unsafe void PrepareSendingBuffer()
        {
            if (preparedSize > 0)
                return;

            sendingBuffer.Reset();

            // 此时sendBuff数据为空
            lock (sendBufferLock)
            {
                TMemoryBufferEx temp = sendBuffer;
                sendBuffer = processingBuffer;
                processingBuffer = temp;
            }

            if (processingBuffer.Size > 0)
            {
                // 此流程后processingBuffer数据被清空
                byte[] packetData = processingBuffer.GetBuffer();
                int headerSize = sizeof(PacketHeader);
                while (processingBuffer.Size > 0)
                {
                    // 压缩,加密
                    int sendPos = processingBuffer.ReadPos;
                    fixed (byte* ptr = &packetData[sendPos])
                    {
                        PacketHeader* orgHeader = (PacketHeader*)ptr;
                        int bodyOffset = sendPos + headerSize;
                        int packetSize = headerSize + orgHeader->bodyLen;
                        processingBuffer.Read(null, 0, packetSize);
                        if (!orgHeader->NeedCompress())
                        {
                            sendingBuffer.Write(packetData, sendPos, packetSize);
                        }
                        else
                        {
                            compressBuff.Reset();
                            compressBuff.Reserve(orgHeader->bodyLen + 400);//QuickLZ内部定义的保留尺寸
                            int compressedLength = QuickLZ.Compress(packetData, bodyOffset, orgHeader->bodyLen, compressBuff.GetBuffer(), 0);
                            if (compressedLength == 0 || compressedLength > orgHeader->bodyLen)
                            {
                                orgHeader->flag = (byte)((int)orgHeader->flag & ~FlagCode.FLAG_COMPRESSED);
                                sendingBuffer.Write(packetData, sendPos, packetSize);
                            }
                            else
                            {
                                orgHeader->bodyLen = compressedLength;
                                packetSize = headerSize + orgHeader->bodyLen;
                                sendingBuffer.Write(packetData, sendPos, headerSize);
                                sendingBuffer.Write(compressBuff.GetBuffer(), 0, compressedLength);
                            }
                        }

                        if (orgHeader->NeedEncrypt())
                        {
                            fixed (byte* encrypt_ptr = &(sendingBuffer.GetBuffer()[sendingBuffer.WritePos - packetSize]))
                            {
                                PacketHeader* newHeader = (PacketHeader*)encrypt_ptr;
                                byte* newBody = encrypt_ptr + headerSize;
                                int keyLen = 4;
                                sendCrypter.Encrypt(newBody, newHeader->bodyLen, newHeader->key, ref keyLen);
                            }
                        }//if
                    }//fixed
                }//while

                processingBuffer.Reset();
            }

            preparedSize = sendingBuffer.Size;
        }

        public void DoSendRequest()
        {
            if (!sendValid || !isConnected)
                return;

            lock (sendingLock)
            {
                if (sendingFlag)
                    return;
                sendingFlag = true;
            }

            PrepareSendingBuffer();

            if (preparedSize > 0)
            {
                try
                {
                    socket.BeginSend(sendingBuffer.GetBuffer(), sendingBuffer.ReadPos, sendingBuffer.Size, 0, OnSendComplete, socket);
                    return;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogDebug("Net:Send:ErrorCode:" + ex.ToString());
                    OnSendError(NetErrors.NE_NET_INTERRUPT);
                }
            }

            lock (sendingLock)
            {
                sendingFlag = false;
            }

            // 无可发送数据
            if (isWaitingClose)
            {
                if (socket != null)
                    socket.Shutdown(SocketShutdown.Send);
                LogManager.Instance.LogInfo("Net:SendComplete:DoShutdownSocketRequest.");
            }
        }

        protected void OnSendComplete(IAsyncResult ar)
        {
            try
            {
                int sendSize = socket.EndSend(ar);
                //LogManager.Instance.LogDebug("发送 {0} 字节.", sendSize);

                sendingBuffer.Read(null, 0, sendSize);

                preparedSize -= sendSize;

                lock (sendingLock)
                {
                    sendingFlag = false;
                }

                DoSendRequest();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogDebug("Net:OnSendComplete:Error:" + ex);
                OnSendError(NetErrors.NE_NET_INTERRUPT);
            }
        }

        protected void DoOnceRecv()
        {
            if (!recvValid || !isConnected)
                return;

            try
            {
                recvBuff.LeftSpace = 8192;
                socket.BeginReceive(recvBuff.buff, recvBuff.WritePos, recvBuff.LeftSpace, 0, OnRecvComplete, socket);
                
            }
            catch (SocketException ex)
            {
                LogManager.Instance.LogDebug("Net:BeginReceive:ErrorCode:" + ex);
                OnRecvError(NetErrors.NE_NET_INTERRUPT);
            }
        }

        protected void OnRecvComplete(IAsyncResult ar)
        {
            try
            {
                if (!socket.Connected)
                {
                    LogManager.Instance.LogDebug("Net:OnRecvComplete:Error");
                    OnRecvError(NetErrors.NE_NET_INTERRUPT);
                    return;
                }

                int recvdBytes = socket.EndReceive(ar);
                if (recvdBytes < 0)
                {
                    LogManager.Instance.LogDebug("Net:OnRecvComplete:Error");
                    OnRecvError(NetErrors.NE_NET_INTERRUPT);
                    return;
                }
                else if (recvdBytes == 0)
                {
                    LogManager.Instance.LogDebug("Net:RecvWork:Remote Point Closed.");
                    OnRecvError(NetErrors.NE_NET_INTERRUPT);
                    return;
                }

                recvBuff.WritePos += recvdBytes;
                unsafe
                {
                    while (recvBuff.size > sizeof(PacketHeader))
                    {
                        fixed (byte* ptr = &recvBuff.buff[recvBuff.ReadPos])
                        {
                            PacketHeader* header = (PacketHeader*)ptr;
                            int packetSize = header->bodyLen + sizeof(PacketHeader);
                            if (packetSize > recvBuff.size)
                                break;
                            byte ret = CheckHeader(recvBuff.buff, recvBuff.ReadPos, packetSize);
                            if (ret == PacketErrorCode.PACKET_OK)
                            {
                                if (!OnBodyComplete(recvBuff.buff, recvBuff.ReadPos))
                                {
                                    LogManager.Instance.LogDebug("Net:RecvWork:OnBodyComplete:Error");
                                    OnRecvError(NetErrors.NE_PACKET_ERROR);
                                    return;
                                }
                                recvBuff.ReadPos += packetSize;
                            }
                            else if (ret == PacketErrorCode.PACKET_ALREADY_RECVD)
                            {
                                recvBuff.ReadPos += packetSize;
                                continue;
                            }
                            else
                            {
                                OnPacketError(ret);
                                return;
                            }
                        }
                    }
                }

                if (recvBuff.PreFreeSpace > 4096)
                    recvBuff.MoveDataToBegin();
                DoOnceRecv();
            }
            catch (SocketException ex)
            {
                LogManager.Instance.LogDebug("Net:OnRecvComplete:Error:" + ex);
                OnRecvError(NetErrors.NE_NET_INTERRUPT);
            }
        }

        protected unsafe bool ProcessRecvdResendData(byte[] resendData, int resendDataOffset)
        {
            TMemoryBufferEx recvdResendDataBuff = new TMemoryBufferEx();
            if (!ProcessPacketBody(resendData, resendDataOffset, recvdResendDataBuff))
                return false;

            byte[] bodyBuffer = recvdResendDataBuff.GetBuffer();
            int size = recvdResendDataBuff.Size;
            int pos = sizeof(PacketHeader);//去除外包的头部,获取内部的重发包
            while (size > sizeof(PacketHeader))
            {
                fixed (byte* ptr = &bodyBuffer[pos])
                {
                    PacketHeader* header = (PacketHeader*)ptr;
                    int packetSize = header->bodyLen + sizeof(PacketHeader);
                    if (packetSize > size)
                        return false;

                    byte ret = CheckHeader(bodyBuffer, pos, packetSize);
                    if (ret == PacketErrorCode.PACKET_OK)
                    {
                        if (!OnBodyComplete(bodyBuffer, pos))
                        {
                            LogManager.Instance.LogDebug("Net:RecvWork:OnBodyComplete:Error");
                            OnRecvError(NetErrors.NE_PACKET_ERROR);
                            return false;
                        }
                        size -= packetSize;
                        pos += packetSize;
                        continue;
                    }
                    else if (ret == PacketErrorCode.PACKET_ALREADY_RECVD)
                    {
                        size -= packetSize;
                        pos += packetSize;
                        continue;
                    }
                    else
                    {
                        OnPacketError(ret);
                        return false;
                    }
                }
            }
            return true;
        }

        protected unsafe int GetPacketSize(byte[] buff, int offset)
        {
            fixed (byte* headerPtr = &buff[offset])
            {
                PacketHeader* header = (PacketHeader*)headerPtr;
                return sizeof(PacketHeader) + header->bodyLen;
            }
        }

        protected unsafe bool IsEventPacket(byte[] buff, int offset)
        {
            fixed (byte* headerPtr = &buff[offset])
            {
                PacketHeader* header = (PacketHeader*)headerPtr;
                return header->flag == FlagCode.FLAG_PACKET_EVENT;
            }
        }

        protected unsafe byte CheckHeader(byte[] buff, int offset, int packetSize)
        {
            fixed (byte* headerPtr = &buff[offset])
            {
                PacketHeader* header = (PacketHeader*)headerPtr;
                byte checkResult = header->CheckHeaderBase();
                if (checkResult != PacketErrorCode.PACKET_OK)
                    return checkResult;

                //LogManager.Instance.LogDebug("<--包:/n 长度: {0} /n 序列号:{1} ,正确序号:{2} /n", header->bodyLen, header->seqNo, recvSeqNoBase + 1);
                if ((header->flag & FlagCode.FLAG_PACKET_REUSE_RESEND_DATA) != 0)
                {
                    if (header->bodyLen > 0)
                    {
                        if (!ProcessRecvdResendData(buff, offset))
                            return PacketErrorCode.PAKCET_UNCOMPRESS_ERROR;
                    }
                    return PacketErrorCode.PACKET_ALREADY_RECVD;
                }

                // 是否会话消息包
                if ((header->flag & FlagCode.FLAG_PATKET_SESSION_KEY) != 0)
                {
                    byte* body = (byte*)header + sizeof(PacketHeader);
                    UInt64 sessionID = *((UInt64*)body);
                    OnSessionKey(sessionID);
                    return PacketErrorCode.PACKET_ALREADY_RECVD;
                }

                // 是重连信号回复
                if ((header->flag & FlagCode.FLAG_PACKET_REUSE_SESSION_RSP) != 0)
                {
                    byte* body = (byte*)header + sizeof(PacketHeader);
                    UInt64 sessionID = *((UInt64*)body);
                    OnReuseSessionRsp(sessionID);
                    return PacketErrorCode.PACKET_ALREADY_RECVD;
                }

                if (header->seqNo > recvSeqNoBase + 1)
                {
                    LogManager.Instance.LogError("!!!包错误:/n 长度: {0} /n 序列号:{1} ,正确序号:{2} /n", header->bodyLen, header->seqNo, recvSeqNoBase + 1);
                    return PacketErrorCode.PACKET_SEQ_NUMBER_ERROR;
                }
                else if (header->seqNo < recvSeqNoBase + 1)
                {
                    return PacketErrorCode.PACKET_ALREADY_RECVD;
                }
                return PacketErrorCode.PACKET_OK;
            }
        }

        protected unsafe bool ProcessPacketBody(byte[] buff, int offset, TMemoryBufferEx outBuff)
        {
            fixed (byte* ptr = &buff[offset])
            {
                PacketHeader* header = (PacketHeader*)ptr;
                int headerSize = sizeof(PacketHeader);
                byte* body = (byte*)header + headerSize;
                int orgBodyLen = header->bodyLen;

                // 解密
                if (header->NeedEncrypt())
                {
                    fixed (byte* recvdCryptKeyPtr = &recvdCryptKey[0])
                    {
                        int keyLen = header->GetKey(body, ref orgBodyLen, ref recvdCryptKey);
                        if (keyLen == PacketHeader.INVALID_KEY_LENGTH ||
                            !recvCrypter.Decrypt(body, orgBodyLen, recvdCryptKeyPtr, keyLen))
                        {
                            OnPacketError(PacketErrorCode.PACKET_DECRYPT_ERROR);
                            return false;
                        }
                    }
                }

                if (header->NeedCompress())
                {
                    header->bodyLen = QuickLZ.GetUncompressedLength(buff, offset + headerSize);
                    header->flag = 0; // 复位标记, 以便与事件头(FLAG_PACKET_EVENT)区别
                    outBuff.Write(buff, offset, headerSize);

                    outBuff.Reserve(header->bodyLen);
                    QuickLZ.Decompress(buff, offset + headerSize, orgBodyLen, outBuff.GetBuffer(), outBuff.WritePos);
                    outBuff.Write(null, 0, header->bodyLen);
                }
                else
                {
                    header->flag = 0; // 复位标记, 以便与事件头(FLAG_PACKET_EVENT)区别
                    outBuff.Write(buff, offset, headerSize + orgBodyLen);
                }
                return true;
            }
        }

        protected byte[] recvdCryptKey = new byte[PacketHeader.MAX_KEY_LENGTH];
        protected TMemoryBufferEx recvdPacketsBuff = new TMemoryBufferEx(16 * 1024);

        //// 机器人版本使用
        //[ThreadStatic]
        //static TMemoryBufferEx packetProcessBuff = null;

        // 客户端版本使用
        TMemoryBufferEx packetProcessBuff = new TMemoryBufferEx(16 * 1024);

        protected unsafe bool OnBodyComplete(byte[] buff, int offset)
        {
            fixed (byte* ptr = &buff[offset])
            {
                PacketHeader* header = (PacketHeader*)ptr;
                recvSeqNoBase++;
                if (header->bodyLen == 0)
                    return true;

                //// 机器人版本使用
                //if (packetProcessBuff == null)
                //    packetProcessBuff = new TMemoryBufferEx(16 * 1024);

                // 解密解压
                if (!ProcessPacketBody(buff, offset, packetProcessBuff))
                    return false;

                lock (recvdPacketsBuff)
                {
                    recvdPacketsBuff.Write(
                        packetProcessBuff.GetBuffer(),
                        packetProcessBuff.ReadPos,
                        packetProcessBuff.Size);
                }
                packetProcessBuff.Reset();
            }
            return true;
        }

        protected void OnPacketError(byte code)
        {
            LogManager.Instance.LogDebug("Net:OnPacketError:Code:" + code.ToString());
            PushFlagPacket(FlagPacketTypes.FPT_DESTORY);
        }

        protected void OnSendError(NetErrors error)
        {
            sendValid = false;
            LogManager.Instance.LogDebug("Net:OnSendError:Code:" + error.ToString());
        }

        protected void OnRecvError(NetErrors error)
        {
            if (isWaitingClose)
            {
                PushFlagPacket(FlagPacketTypes.FPT_DESTORY);
                return;
            }

            Close();
            PushFlagPacket(FlagPacketTypes.FPT_DISCONNECT);
        }

        protected virtual void OnSessionKey(UInt64 sessionKey)
        {
            this.sessionKey = sessionKey;
        }

        protected unsafe void CheckResendBuff(string step)
        {
            if (resendBuffer.Size <= 0)
                return;

            byte[] byteBuff = resendBuffer.GetBuffer();
            int pos = resendBuffer.ReadPos;
            int endPos = resendBuffer.WritePos;
            while (pos < endPos)
            {
                if ((endPos - pos) < sizeof(PacketHeader))
                {
                    LogManager.Instance.LogError(step + ":----->重发缓冲数据错误");
                    LogManager.Instance.LogError((new System.Diagnostics.StackTrace()).ToString());
                    return;
                }
                fixed (byte* ptr = &byteBuff[pos])
                {
                    PacketHeader header = *((PacketHeader*)ptr);
                    if (header.bodyLen > resendBuffer.Size)
                    {
                        LogManager.Instance.LogError(step + ":----->重发缓冲数据错误: 1");
                        LogManager.Instance.LogError((new System.Diagnostics.StackTrace()).ToString());
                        return;
                    }
                    pos += sizeof(PacketHeader) + header.bodyLen;
                }
            }
        }

        protected void RestoreSendData(NetLinkBase offlineServer)
        {
            lock (sendBufferLock)
            {
                sendBuffer.Reset();

                resendBuffer.Clone(offlineServer.resendBuffer);
                resendBuffSizeOnPing.Clone(offlineServer.resendBuffSizeOnPing);
                resendDataSizeOnPing = offlineServer.resendDataSizeOnPing;

                //DebugHelper.LogWarning("RestoreSendData:" + resendBuffer.Size + ":" + offlineServer.resendBuffer.Size);
                //CheckResendBuff("RestoreSendData");

                // 将重发数据整包压缩发送(2014.6.13)
                if (resendBuffer.Size > 0)
                {
                    unsafe
                    {
                        PacketHeader header;
                        byte* headerPtr = (byte*)&header;

                        header.bodyLen = resendBuffer.Size;
                        header.flag = FlagCode.FLAG_PACKET_REUSE_RESEND_DATA | FlagCode.FLAG_COMPRESSED;
                        sendBuffer.WriteUnsafeBuff(headerPtr, 0, sizeof(PacketHeader));
                    }
                    sendBuffer.Write(
                        resendBuffer.GetBuffer(),
                        resendBuffer.ReadPos,
                        resendBuffer.Size);
                }

                sendSeqNoBase = offlineServer.sendSeqNoBase;
                pingSeqNo = offlineServer.pingSeqNo;
                lastCollectedPingSeq = offlineServer.lastCollectedPingSeq;

                isEncrypt = offlineServer.isEncrypt;
                isCompress = offlineServer.isCompress;
            }

            OnSendRequest();
        }

        // 服务器回复重连成功,恢复接收序号,替换session key,完成移交.
        protected void RestoreRecvData(NetLinkBase offlineServer)
        {
            if (offlineServer.isConnected)
                offlineServer.Close();

            recvSeqNoBase = offlineServer.recvSeqNoBase;
        }

        public enum LinkStatus
        {
            INIT,
            CONNECTED,
            DISCONNECTED,
            DESTROYED,
        }

        protected LinkStatus linkStatus = LinkStatus.INIT;

        protected const int flagPacketSize = 1 + sizeof(UInt64);
        public void PushFlagPacket(FlagPacketTypes type)
        {
            PushFlagPacket(type, 0);
        }
        protected unsafe void PushFlagPacket(FlagPacketTypes type, UInt64 flagData)
        {
            PacketHeader header;
            header.bodyLen = flagPacketSize;
            header.flag = FlagCode.FLAG_PACKET_EVENT;

            byte flag = (byte)type;
            lock (recvdPacketsBuff)
            {
                byte* headerPtr = (byte*)&header;
                byte* bodyFlag = &flag;
                byte* bodyFlagData = (byte*)&flagData;
                recvdPacketsBuff.WriteUnsafeBuff(headerPtr, 0, sizeof(PacketHeader));
                recvdPacketsBuff.WriteUnsafeBuff(bodyFlag, 0, 1);
                recvdPacketsBuff.WriteUnsafeBuff(bodyFlagData, 0, sizeof(UInt64));
            }
        }

        public List<ILinkListenner> Listenners = new List<ILinkListenner>();
        public OnMessageHandler MsgHandler;
        TMemoryBufferEx onePacketBuff = new TMemoryBufferEx(8192);

        int packetHandleCount = 0;
        public unsafe void HandlePackets()
        {
            packetHandleCount++;
            if (NeedKeepAlive)
            {
                CheckAlive();
                KeepAlive();
            }
            if (MsgHandler == null)
                return;

            int packetSize = 0;
            int packetBodyLen = 0;
            int headerSize = sizeof(PacketHeader);
            int bodyPos = headerSize;
            bool isFlagPacket = false;
            byte[] newPacketData = null;
            while (true)
            {
                lock (recvdPacketsBuff)
                {
                    if (recvdPacketsBuff.Size <= 0)
                        break;
                    packetSize = GetPacketSize(recvdPacketsBuff.GetBuffer(), recvdPacketsBuff.ReadPos);
                    isFlagPacket = IsEventPacket(recvdPacketsBuff.GetBuffer(), recvdPacketsBuff.ReadPos);
                    onePacketBuff.Reset();
                    onePacketBuff.Reserve(packetSize);
                    recvdPacketsBuff.Read(onePacketBuff.GetBuffer(), 0, packetSize);
                    onePacketBuff.Write(null, 0, packetSize);
                }

                packetBodyLen = packetSize - headerSize;
                onePacketBuff.Read(null, 0, headerSize);
                newPacketData = onePacketBuff.GetBuffer();

                fixed (byte* body = &newPacketData[bodyPos])
                {
                    // 连接维护包
                    if (isFlagPacket && (packetBodyLen == flagPacketSize))
                    {
                        switch ((FlagPacketTypes)newPacketData[bodyPos])
                        {
                            case FlagPacketTypes.FPT_CONNECT_OK:
                                if (linkStatus == LinkStatus.CONNECTED)
                                    continue;
                                if (needCheckValid)
                                    SendValidCheck();
                                OnConnected(true);
                                continue;

                            case FlagPacketTypes.FPT_CONNECT_FAILED:
                                OnConnected(false);
                                continue;

                            case FlagPacketTypes.FPT_DISCONNECT:
                                if (linkStatus == LinkStatus.DISCONNECTED)
                                    continue;
                                OnDisconnect();
                                continue;

                            case FlagPacketTypes.FPT_DESTORY:
                                if (linkStatus == LinkStatus.DESTROYED)
                                    continue;
                                OnDestory();
                                continue;

                            case FlagPacketTypes.FPT_REUSE_SUCCESS:
                                {
                                    UInt64* sessionIDPtr = (UInt64*)(body + 1);

                                    NetClientLink offlineServer = NetManager.Instance.GetClientLinkSession(*sessionIDPtr);
                                    if (offlineServer != null)
                                    {
                                        OnReuseSessionSuccess(offlineServer.SessionKey);
                                        RestoreSendData(offlineServer);

                                        OnSessionKey(offlineServer.sessionKey);
                                        offlineServer.sessionKey = 0;
                                        offlineServer.Disconnect();
                                    }
                                    reusing = false;
                                    continue;
                                }

                            case FlagPacketTypes.FPT_REUSE_FAILED:
                                {
                                    UInt64* sessionIDPtr = (UInt64*)(body + 1);

                                    OnReuseSessionFailed(*sessionIDPtr);
                                    reusing = false;
                                    continue;
                                }

                            default:
                                break;
                        }
                    }
                    else if (packetBodyLen == 8)
                    {
                        uint flag = ((uint*)body)[0];
                        if (flag == 0xecceccff)
                        {
                            uint pingSeq = ((uint*)body)[1];
                            SendPong(pingSeq);
                            continue;
                        }
                        else if (flag == 0xecceccfe)
                        {
                            uint pingSeq = ((uint*)body)[1];
                            lastAliveRecvTick = TickerPolicy.Ticker.GetTick();
                            OnPong(lastAliveRecvTick, pingSeq);
                            //DebugHelper.Log(linkID + ":" + lastAliveRecvTick);
                            continue;
                        }
                    }
                }

                // 数据包处理
                MsgHandler(linkID, onePacketBuff);
            }
        }

        bool reusing = false;
        public bool Reuse(NetClientLink offlineServer)
        {
            if (!reusable)
                return false;
            reusing = true;

            return SendReuseSessionData(offlineServer.SessionKey);
        }

        protected bool SendReuseSessionData(UInt64 sessionKeyToReuse)
        {
            lock (sendBufferLock)
            {
                if (!sendValid || !isConnected)
                    return false;

                int headerSize = 0;
                unsafe
                {
                    // 填充包头
                    PacketHeader header;
                    headerSize = sizeof(PacketHeader);

                    byte* headerPtr = (byte*)&header;
                    header.bodyLen = sizeof(UInt64);
                    header.flag = FlagCode.FLAG_PACKET_REUSE_SESSION;

                    //LogManager.Instance.LogDebug("------->包, 长度: {0} 序列号:{1} ,正确序号:{2}", header.bodyLen, header.seqNo, sendSeqNoBase);

                    sendBuffer.WriteUnsafeBuff(headerPtr, 0, headerSize);
                    sendBuffer.WriteUnsafeBuff((byte*)&sessionKeyToReuse, 0, sizeof(UInt64));
                }
            }
            OnSendRequest();
            return true;
        }

        protected bool SendValidCheck()
        {
            lock (sendBufferLock)
            {
                if (!sendValid || !isConnected)
                    return false;

                int headerSize = 0;
                unsafe
                {
                    // 填充包头
                    PacketHeader header;
                    headerSize = sizeof(PacketHeader);

                    byte* headerPtr = (byte*)&header;
                    header.bodyLen = sizeof(Int64);
                    header.flag = FlagCode.FLAG_PATKET_CONNECT_CHECK;
                    sendBuffer.WriteUnsafeBuff(headerPtr, 0, headerSize);

                    Int64 validCheckKey = 12345;
                    sendBuffer.WriteUnsafeBuff((byte*)&validCheckKey, 0, sizeof(Int64));
                }
            }
            OnSendRequest();
            return true;
        }

        public IPEndPoint LocalEndPoint
        {
            get { return (IPEndPoint)socket.LocalEndPoint; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return (IPEndPoint)socket.RemoteEndPoint; }
        }
        #region 监听函数,子类扩展
        public delegate void SendRequestHandler(NetLinkBase link);
        public SendRequestHandler SendReqHandler;
        protected void OnSendRequest()
        {
            SendReqHandler?.Invoke(this);
        }

        protected virtual void OnReuseSessionRsp(ulong sessionID) { }

        protected virtual void OnConnected(bool isOk)
        {
            if (linkStatus == LinkStatus.CONNECTED)
                return;

            linkStatus = LinkStatus.CONNECTED;
            lastAliveRecvTick = TickerPolicy.Ticker.GetTick();
            lastAliveSendTick = lastAliveRecvTick - 3000;
        }
        protected virtual void OnDisconnect()
        {
            linkStatus = LinkStatus.DISCONNECTED;
        }

        protected virtual void OnDestory()
        {
            if (linkStatus == LinkStatus.DESTROYED)
                return;
            else if (linkStatus != LinkStatus.DISCONNECTED &&
                !isWaitingClose)
            {
                OnDisconnect();
            }

            linkStatus = LinkStatus.DESTROYED;
            Close();
        }

        protected virtual void OnReuseSessionSuccess(UInt64 otherSessionKey) { }

        protected virtual void OnReuseSessionFailed(UInt64 otherSessionKey) { }
        #endregion
    }
}
